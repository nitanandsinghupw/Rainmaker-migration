using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using VoiceElements.Client;
using VoiceElements.Common;
using Rainmaker.Common.DomainModel;

namespace Rainmaker.RainmakerDialer
{
    public class PromptRecorder
    {
        #region Variables & Properties

        // References the main Log File created at startup
        private static Log Log = DialerEngine.Log;

        private ManagedChannel managedChannel = null;
        public ManagedChannel ManagedChannel
        {
            get { return managedChannel; }
            set { managedChannel = value; }
        }

        private VoiceResource m_VoiceResource = null;
        public VoiceResource VoiceResource
        {
            get { return m_VoiceResource; }
            set { m_VoiceResource = value; }
        }

        private AutoResetEvent threadEvent = new AutoResetEvent(false);

        private bool isDialing = false;

        private bool m_TerminateCall = false;
        public bool TerminateCall
        {
            get { return m_TerminateCall; }
            set
            {
                Log.Write("Terminate Call Set to: " + value.ToString());            
                lock (this)
                {
                    m_TerminateCall = value;
                }
                threadEvent.Set();
            }
        }

        public DialResult CurrentDialResult;

        private string m_NumberToCall = "";
        private string m_PromptPath = "";
        private string m_RecordPath = "";

        #endregion

        #region Constructor
        public PromptRecorder(string targetNumber)
        {
            m_NumberToCall = targetNumber;
            try
            {
                m_PromptPath = Utilities.GetAppSetting("RecorderAudioPath", "");
                m_RecordPath = Utilities.GetAppSetting("PromptRecordingPath", "");
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error retreieving prompt recorder settings.");
            }
        }

        #endregion

        #region MainScript
        public void RunScript()
        {

            try
            {
                if (string.IsNullOrEmpty(m_RecordPath) || string.IsNullOrEmpty(m_PromptPath))
                {
                    Log.Write("Error in prompt recorder: invalid or missing paths to play or record.  Aborting.");
                    return;
                }
                if (m_NumberToCall.Length < 7)
                {
                    Log.Write("Invalid number to call for prompt recording:{0}.  Aborting.", m_NumberToCall);
                    return;
                }
                // *** Looks like it will give a channel with campaign = 0, but confirm
                ManagedChannel = ManagedChannel.GetOutboundChannel(0);

                if (ManagedChannel == null || ManagedChannel.ChannelResource == null)
                {
                    // Channels not available, prior checking also implemented this will not happen
                    Log.Write("No channels available, recorder thread has been started without a channel.");
                    return;
                }

                // Subscribe to hangup events
                try
                {
                    ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                }
                catch { }
                ManagedChannel.ChannelResource.Disconnected += new Disconnected(ChannelResource_Disconnected);

                // No CPA, only wait for connect
                ManagedChannel.ChannelResource.CallProgress = CallProgress.WaitForConnect;

                if (ManagedChannel.ChannelResource is T1Channel) // For t1, route to self
                {
                    ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                }

                DialResult dr;

                ManagedChannel.ChannelResource.CallProgressTemplate = @"Dialogic\DxCap";
                Dictionary<string, int> overrides = new Dictionary<string, int>();
                overrides.Add("ca_noanswer", 3600);
                ManagedChannel.ChannelResource.CallProgressOverrides = overrides;

                Log.WriteWithId(ManagedChannel.Id.ToString(), "Recorder Dialing {0} on channel {1}.", m_NumberToCall, ManagedChannel.ChannelResource.DeviceName);

                try
                {
                    lock (this) { isDialing = true; }
                    dr = ManagedChannel.ChannelResource.Dial(m_NumberToCall);
                    lock (this) { isDialing = false; }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "Recorder - General dialing exception.");
                    dr = DialResult.Failed;
                }
                finally
                {
                    ManagedChannel.ChannelResource.CallProgress = CallProgress.WaitForConnect;
                }

                // Log dial result
                Log.WriteWithId(ManagedChannel.Id.ToString(), "The recorder dial result for {0} was: {1}.", m_NumberToCall, dr);
                switch (dr)
                {
                    case DialResult.Connected:
                    case DialResult.HumanDetected:
                    case DialResult.Successful:
                    case DialResult.MachineDetected:
                        RecordPrompts();
                        break;
                    case DialResult.NoAnswer:
                    case DialResult.Busy:
                    case DialResult.Error:
                    case DialResult.Failed:
                    case DialResult.OperatorIntercept:
                    default:
                        // call failed no answer
                        Log.WriteWithId(ManagedChannel.Id.ToString(), "Recorder call failed with dial result: '{0}'", dr);
                        break;
                }
                return;
            }
            catch (ElementsException ee)
            {
                // These are Telephony Specific exceptions, such an the caller hanging up the phone during a play or record.
                if (ee is HangupException)
                {
                    Log.Write("The Recorder Hungup.");
                }
                else
                    Log.WriteException(ee, "Recorder Elements Exception");
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error in prompt recorder main script");
            }
            finally
            {
                if (ManagedChannel != null)
                {
                    try
                    {
                        if (isDialing)
                        {
                            try { ManagedChannel.ChannelResource.StopDial(); }
                            catch { }
                            isDialing = false;
                        }
                        if (ManagedChannel.ChannelResource != null)
                        {
                            ManagedChannel.ChannelResource.Disconnect();        
                            ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                            // Route to Home VR.
                            ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                        }
                    }
                    catch { }
                    // Add channel back to the list for later use // 0 as camp ID looks ok once again, testing will show.
                    ManagedChannel.ReturnChannel(ManagedChannel, 0);
                }
            }
        }

        

        #endregion


        #region Main Recording Methods
        private void RecordPrompts()
        {
            TerminationCode tc;
            bool responseReceived = false;
            string userInput = string.Empty;
            string fileToPlay = string.Empty;
            string recordFile = string.Empty;
            bool inRecordingLoop = true;
            bool deleteFile = false;

            m_VoiceResource = managedChannel.ChannelResource.VoiceResource;

            try 
	        {	        
		        for (int i = 0; i < 3; i++)// Play intro, get a 1 to confirm they want to record now.
                {
                    fileToPlay = Path.Combine(m_PromptPath, "FileIntro.wav");
                    m_VoiceResource.MaximumTime = 10;
                    m_VoiceResource.MaximumDigits = 1;
                    m_VoiceResource.InterDigitTimeout = 5;
                    m_VoiceResource.TerminationDigits = "@";
                    m_VoiceResource.Play(fileToPlay);
                    tc = m_VoiceResource.GetDigits();
                    userInput = m_VoiceResource.DigitBuffer;
                    Log.Write("Prompt recorder user input: '{0}'", userInput);
                    if (userInput == "1")
                    {
                        responseReceived = true;
                        break;
                    }
                }

                if (!responseReceived)
                {
                    EndCall();
                    return;
                }

                while (inRecordingLoop)
                {
                    responseReceived = false;

                    for (int i = 0; i < 3; i++)// Get numeric file name.
                    {
                        fileToPlay = Path.Combine(m_PromptPath, "RecFileNum.wav");
                        m_VoiceResource.MaximumTime = 40;
                        m_VoiceResource.MaximumDigits = 15;
                        m_VoiceResource.InterDigitTimeout = 8;
                        m_VoiceResource.TerminationDigits = "#";
                        m_VoiceResource.Play(fileToPlay);
                        tc = m_VoiceResource.GetDigits();
                        userInput = m_VoiceResource.DigitBuffer;
                        Log.Write("Prompt recorder user input (filename): '{0}'", userInput);
                        if (userInput.Length > 0)
                        {
                            responseReceived = true;
                            break;
                        }
                    }

                    if (!responseReceived)
                    {
                        EndCall();
                        return;
                    }

                    recordFile = Path.Combine(m_RecordPath, string.Format("{0}.wav", userInput));

                    if (File.Exists(recordFile))
                    {
                        // file exists, prompt to delete
                        for (int i = 0; i < 3; i++)// Play intro, get a 1 to confirm they want to record now.
                        {
                            fileToPlay = Path.Combine(m_PromptPath, "RecIntro.wav");
                            m_VoiceResource.MaximumTime = 10;
                            m_VoiceResource.MaximumDigits = 1;
                            m_VoiceResource.InterDigitTimeout = 5;
                            m_VoiceResource.TerminationDigits = "@";
                            m_VoiceResource.Play(fileToPlay);
                            tc = m_VoiceResource.GetDigits();
                            userInput = m_VoiceResource.DigitBuffer;
                            Log.Write("Prompt recorder user input: '{0}'", userInput);
                            if (userInput == "1")
                            {
                                deleteFile = true;
                                break;
                            }
                        }

                        if (userInput.Length < 1)
                        {
                            EndCall();
                            return;
                        }

                        if (!deleteFile)
                        {
                            continue;
                        }

                        File.Delete(recordFile);
                        fileToPlay = Path.Combine(m_PromptPath, "RecDeleted.wav");
                    }

                    // Record the file
                    m_VoiceResource.TerminationDigits = "";
                    fileToPlay = Path.Combine(m_PromptPath, "RecReady.wav");
                    m_VoiceResource.Play(fileToPlay);

                    m_VoiceResource.MaximumTime = 180;
                    m_VoiceResource.MaximumDigits = 1;
                    m_VoiceResource.InterDigitTimeout = 8;
                    m_VoiceResource.TerminationDigits = "#";
                    m_VoiceResource.Record(recordFile);

                    responseReceived = false;

                    for (int i = 0; i < 3; i++)// Play intro, get a 1 to confirm they want to record now.
                    {
                        fileToPlay = Path.Combine(m_PromptPath, "RecAnother.wav");
                        m_VoiceResource.MaximumTime = 10;
                        m_VoiceResource.MaximumDigits = 1;
                        m_VoiceResource.InterDigitTimeout = 5;
                        m_VoiceResource.TerminationDigits = "@";
                        m_VoiceResource.Play(fileToPlay);
                        tc = m_VoiceResource.GetDigits();
                        userInput = m_VoiceResource.DigitBuffer;
                        Log.Write("Prompt recorder user input: '{0}'", userInput);
                        if (userInput == "1")
                        {
                            responseReceived = true;
                            break;
                        }
                    }

                    if (!responseReceived)
                    {
                        EndCall();
                        return;
                    }
                    continue;
                }

	        }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error ending prompt record IVR script.");
            }
        }

        private void EndCall()
        {
            try
            {
                //TerminationCode tc;
                //bool responseReceived = false;
                string userInput = string.Empty;
                string fileToPlay = string.Empty;

                // Play intro, get a 1 to confirm they want to record now.
                fileToPlay = Path.Combine(m_PromptPath, "RecGBye.wav");
                m_VoiceResource.TerminationDigits = "";
                m_VoiceResource.Play(fileToPlay);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error ending prompt record call.");
            }
            return;
        } 
        #endregion


        #region Event Handling
        void ChannelResource_Disconnected(object sender, DisconnectedEventArgs e)
        {
            try
            {
                TerminateCall = true;

                if (isDialing)
                {
                    try { ManagedChannel.ChannelResource.StopDial(); }
                    catch { }
                    isDialing = false;
                }

                // Here we will simply write to the log that the caller hung up the phone.
                Log.Write("Recorder Channel Disconnected Event Received");
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "ChannelResource_Disconnected Exception");
            }
        } 
        #endregion

    }
}
