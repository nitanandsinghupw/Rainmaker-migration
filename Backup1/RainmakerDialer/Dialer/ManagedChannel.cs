using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VoiceElements.Common;
using VoiceElements.Client;

namespace Rainmaker.RainmakerDialer
{

    public class ManagedChannel
    {
        
        #region LocalData Channel specific

        // Holds ChannelResource
        private ChannelResource channelResource;
        public ChannelResource ChannelResource
        {
            get { return channelResource; }
            set { channelResource = value; }
        }

        // Holds Channel Type 
        private ChannelType type;
        public ChannelType Type
        {
            get { return type; }
            set { type = value; }
        }

        // Holds Channel status
        private ChannelStatus status;
        public ChannelStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        // Holds id of this Channel
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        // Holds id of this Channel
        private bool connected = false;
        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="channelResource"></param>
        public ManagedChannel(ChannelResource channelResource)
        {

           
            if (!(channelResource is MsiChannel))
            {

                channelResource.VoiceResource = DialerEngine.TelephonyServer.GetVoiceResource();

                Log.Write("|MC|Channel assignment of voice resource {0} to channel {1}", channelResource.VoiceResource.DeviceName, channelResource.DeviceName);

                channelResource.RouteFull(channelResource.VoiceResource);

                // Tells the server what Vap File to use for this call
                channelResource.VoiceResource.VapFile = @"english.vap";                     

                channelResource.VoiceResource.Codec = Codec.PCM_8Khz_8Bit;                  

            }


            // Instruct the server to tell us if a human or machine answers the phone.
            //ManagedChannel.ChannelResource.CallProgress = CallProgress.AnalyzeCall;

            // Instruct the server to wait no more then 60 seconds for a connection
            channelResource.MaximumTime = Convert.ToInt32(Utilities.GetAppSetting("CustomerConnectionTimeout", "60"));
            this.ChannelResource = channelResource;
            this.Type = GetChannelType(channelResource);
            this.Status = ChannelStatus.Available;
            this.ChannelResource.NewCall += new NewCall(ChannelResource_NewCall);
            try
            {
                this.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
            }
            catch { }
            this.ChannelResource.Disconnected += new Disconnected(ChannelResource_Disconnected);   
        }

        void ChannelResource_Disconnected(object sender, DisconnectedEventArgs e)
        {

            if (this.ChannelResource is MsiChannel)                                 
            {
                Log.Write("|MC|Msi {0} went on hook", this.ChannelResource.DeviceName);   
            }
            else
            {
                Log.Write("|MC|Channel {0} disconnected.", this.ChannelResource.DeviceName);   
            }
        }

        /// <summary>
        /// New call event for channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChannelResource_NewCall(object sender, NewCallEventArgs e)
        {
            if (e.ChannelResource is MsiChannel)
            {
                Log.Write("|MC|Msi {0} went off hook", e.ChannelResource.DeviceName);   

                // Start something here to managed an off hook agent...

                //Check if the agent is logged in or not and play message 
                //to agent to Login else add to off hook list
                ManagedAgent ma = ManagedAgent.CheckAgentLoggedIn(e.ChannelResource.DeviceName);   
                ManagedChannel mc = null;

                if (ma != null)
                {
                    if (!ma.AgentDetails.AllwaysOffHook)
                    {
                        foreach (ManagedChannel mc2 in ManagedChannel.MsiChannelList)
                        {
                            if (mc2.ChannelResource.Equals(e.ChannelResource))
                            {
                                mc = mc2;
                                break;
                            }
                        }

                        if (mc == null)
                        {
                            Log.Write("|MC|ChannelResource_NewCall unable to locate channel.");
                        }
                        else
                        {
                            ma.InboundAgent(mc);
                        }
                    }
                    else
                    {
                        Log.Write("|MC|ChannelResource_NewCall AllwaysOffHook agent with station-{0}", e.ChannelResource.DeviceName);
                    }
                }
                else
                {
                    try
                    {
                        
                        // VoiceResource on MSIChannel, 
                        // e.ChannelResource.VoiceResource.PlayTTS("Please Login");
                    }
                    finally
                    {
                        e.ChannelResource.Disconnect();
                    }
                }
            }
            else
            {
                // Handle new call event
                try
                {
                    Log.Write("|MC|{0} NewCall. Answering.", e.ChannelResource.DeviceName);   // MONROE 
                    e.ChannelResource.Answer();                                             
                    Log.Write("|MC|{0} Playing Good Bye Message.", e.ChannelResource.DeviceName);  
                    e.ChannelResource.VoiceResource.Play(@"AudioFiles\Goodbye.wav");         
                }
                finally
                {
                    Log.Write("|MC|{0} Disconnecting.", e.ChannelResource.DeviceName);       
                    e.ChannelResource.Disconnect();                                      
                }
            }
        }

        #endregion

        #region Static variables

        // References the main Log File created at startup
        private static Log Log = DialerEngine.Log;

        // Reference to msi channels
        private static List<ManagedChannel> MsiChannelList = new List<ManagedChannel>();

        // Reference to t1/sip/digital channel list 
        private static List<ManagedChannel> OutboundChannelList = new List<ManagedChannel>();

        // Reference to Campaign channels count
        private static Dictionary<long, int> dCampaignChannelCount = new Dictionary<long, int>();

        // lock variable
        private static object s_SyncVar = new object();

        private static int AvailableOutboundCount = 0;

        #endregion

        #region Static Methods

        /// <summary>
        /// Sets All channels
        /// </summary>
        /// <param name="ChannelList"></param>
        /// <returns></returns>
        public static void Initialize()
        {
            Log.Write("|MC|Initializing managed channels.");
            List<ChannelResource> ChannelList = null;
            try
            {
                ChannelList = DialerEngine.TelephonyServer.GetAllChannels();
                int iMaxAllowedForAgents = Convert.ToInt32(Utilities.GetAppSetting("MaxAgentChannelCount", "2"));
                if (ChannelList.Count <= iMaxAllowedForAgents)
                {
                    // Incorrect allocation dispose all channels 
                    DisposeChannels(ChannelList);
                    throw new Exception("No sufficiant channels available");
                }

                for (int i = 0; i < ChannelList.Count; i++)
                {
                    Log.Write("|MC|Managed channel list adding device '{0}'", ChannelList[i].DeviceName);       
                    ManagedChannel managedChannel = new ManagedChannel(ChannelList[i]);
                    managedChannel.Id = i + 1;
                    if (managedChannel.Type == ChannelType.MsiChannel)
                    {
                        MsiChannelList.Add(managedChannel);
                    }
                    else if (managedChannel.Type == ChannelType.SipChannel |
                            managedChannel.Type == ChannelType.T1Channel |
                            managedChannel.Type == ChannelType.DigitalChannel)
                    {
                        OutboundChannelList.Add(managedChannel);

                        AvailableOutboundCount++;
                    }
                }
            }
            catch (Exception ee)
            {
                Log.WriteException(ee, "SetChannels Exception");
                if (ChannelList != null)
                    DisposeChannels(ChannelList);
                throw ee;
            }
        }

        /// <summary>
        /// Get outbound channel
        /// </summary>
        /// <param name="campaignID"></param>
        /// <returns></returns>
        public static ManagedChannel GetOutboundChannel(long campaignID)
        {
            //Log.Write("|MC|GetOutboundChannel");
            ManagedChannel mc = null;
            lock (s_SyncVar)
            {
                try
                {
                    if (OutboundChannelList.Count > 0)
                    {
                        for (int i = 0; i < OutboundChannelList.Count; i++)
                        {
                            if (OutboundChannelList[i].Status == ChannelStatus.Available)
                            {
                                AvailableOutboundCount--;
                                OutboundChannelList[i].Status = ChannelStatus.Busy;
                                mc = OutboundChannelList[i];
                                mc.ChannelResource.VoiceResource.Codec = Codec.MULAW_8Khz_8Bit;
                                break;
                            }
                        }

                        // increment the count for this campaign
                        if (mc != null && campaignID > 0)
                        {
                            int count = 1;
                            if (dCampaignChannelCount.ContainsKey(campaignID))
                            {
                                count = dCampaignChannelCount[campaignID];
                                count++;
                                dCampaignChannelCount.Remove(campaignID);
                            }
                            dCampaignChannelCount.Add(campaignID, count);
                        }
                    }
                }
                catch (Exception ee)
                {
                    Log.WriteException(ee, "GetChannel Exception");
                }
            }
            return mc;
        }

        /// <summary>
        /// Return channel back to available list
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="campaignID"></param>
        public static void ReturnChannel(ManagedChannel mc, long campaignID)
        {
            Log.Write("|MC|0|Returning channel '{1}' to available channel pool.", campaignID, mc.ChannelResource.DeviceName);
            mc.Connected = false;
            lock (s_SyncVar)
            {
                try
                {
                    try
                    {
                        if (mc.ChannelResource != null)
                        {
                            mc.ChannelResource.Disconnect();
                        }
                    }
                    catch { }

                    if (mc.type != ChannelType.MsiChannel)
                    {

                        for (int i = 0; i < OutboundChannelList.Count; i++)
                        {
                            if (OutboundChannelList[i].Id == mc.Id)
                            {
                                OutboundChannelList[i].Status = ChannelStatus.Available;
                                AvailableOutboundCount++;

                                // decrement the count for this campaign
                                if (campaignID > 0)
                                {
                                    int iCount = 0;
                                    if (dCampaignChannelCount.ContainsKey(campaignID))
                                    {
                                        iCount = dCampaignChannelCount[campaignID];
                                        iCount--;
                                        dCampaignChannelCount.Remove(campaignID);
                                    }
                                    dCampaignChannelCount.Add(campaignID, iCount);
                                }

                                return;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < MsiChannelList.Count; i++)
                        {
                            if (MsiChannelList[i].Id == mc.Id)
                            {
                                MsiChannelList[i].Status = ChannelStatus.Available;
                                return;
                            }
                        }
                    }
                }
                catch (Exception ee)
                {
                    Log.WriteException(ee, "ReturnChannel Exception");
                }
            }
        }

        /// <summary>
        /// Get msi channel
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public static ManagedChannel GetMsiChannel(string channelName)
        {
            ManagedChannel mc = null;
            lock (MsiChannelList)
            {
                try
                {
                    for (int i = 0; i < MsiChannelList.Count; i++)
                    {
                        if (MsiChannelList[i].Status == ChannelStatus.Available 
                            && MsiChannelList[i].ChannelResource.DeviceName.ToLower() == channelName.ToLower()) 
                        {
                            MsiChannelList[i].Status = ChannelStatus.Busy;
                            mc = MsiChannelList[i];
                            //mc.ChannelResource.VoiceResource.Codec = Codec.PCM_8Khz_8Bit;         
                            break;
                        }
                    }
                }
                catch (Exception ee)
                {
                    Log.WriteException(ee, "GetMsiChannel Exception");
                }
            }
            //
            Log.Write("|MC|GetMsiChannel check the device name and route to : {0} ", 
                mc == null ? "--No MSI Channel Available--" : mc.ChannelResource.DeviceName); 
            return mc;
        }

        /// <summary>
        /// Get msi channel
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public static bool IsMsiChannelAvailable(string channelName)
        {
            bool bAvailable = false;
            lock (MsiChannelList)
            {
                try
                {
                    for (int i = 0; i < MsiChannelList.Count; i++)
                    {
                        if (MsiChannelList[i].ChannelResource.DeviceName.ToLower() == channelName.ToLower())
                        {
                            bAvailable = true;
                            break;
                        }
                    }
                }
                catch (Exception ee)
                {
                    Log.WriteException(ee, "GetMsiChannel Exception");
                }
            }
            if (!bAvailable)
            {
                Log.Write("|MC|Invalid MSI Channel Name : {0}", channelName);
            }
            //Log.Write("|MC|IsMsiChannelAvailable({0}) returning: {1}", channelName, bAvailable);
            return bAvailable;
        }

        /// <summary>
        /// returns available channel count
        /// </summary>
        /// <returns></returns>
        public static int GetAvailableOutboundCount()
        {
            lock (s_SyncVar)
            {
                return AvailableOutboundCount;
            }
        }

        /// <summary>
        /// Returns channels available or not
        /// </summary>
        /// <returns></returns>
        public static bool IsChannelsAvailable()
        {
            lock (s_SyncVar)
            {
                //Log.Write("|MC|Available channel count : {0}", AvailableOutboundCount);
                return (AvailableOutboundCount > 0);
            }
        }

        /// <summary>
        /// Returns channels available or not with respect to campaign
        /// </summary>
        /// <param name="campaignID"></param>
        /// <param name="iMaxAllowed"></param>
        /// <returns></returns>
        public static bool IsChannelsAvailable(long campaignID, int iMaxAllowed)
        {
            // first check if channels exists or not for calling
            if (!IsChannelsAvailable())
                return false;

            // check how many assignd for this campaign with max allowed
            bool bAvailable = true;
            lock (s_SyncVar)
            {
                int iAssignd = 0;
                try
                {
                    iAssignd = dCampaignChannelCount[campaignID];
                }
                catch { }
                bAvailable = (iMaxAllowed > iAssignd);
            }
            return bAvailable;
        }

        /// <summary>
        /// Returns type of the channel
        /// </summary>
        /// <param name="cr"></param>
        /// <returns></returns>
        private static ChannelType GetChannelType(ChannelResource cr)
        {
            if (cr is MsiChannel)
                return ChannelType.MsiChannel;
            if (cr is SipChannel)
                return ChannelType.SipChannel;
            if (cr is T1Channel)
                return ChannelType.T1Channel;
            if (cr is DigitalChannel)
                return ChannelType.DigitalChannel;

            return ChannelType.T1Channel;
        }

        /// <summary>
        /// Dispose All Channels
        /// </summary>
        public static void Dispose()
        {
            DisposeChannels(MsiChannelList);
            DisposeChannels(OutboundChannelList);
            dCampaignChannelCount.Clear();
            AvailableOutboundCount = 0;
        }

        /// <summary>
        /// Dispose Channels
        /// </summary>
        /// <param name="ChannelList"></param>
        /// <returns></returns>
        private static void DisposeChannels(List<ManagedChannel> ChannelList)
        {
            try
            {
                for (int i = 0; i < ChannelList.Count; i++)
                {

                    try { ChannelList[i].ChannelResource.Disconnect(); }
                    catch { }
                    try { ChannelList[i].ChannelResource.Dispose(); }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Error disposing");
                    }
                }
                ChannelList.Clear();
            }
            catch (Exception ee)
            {
                Log.WriteException(ee, "DisposeChannels Exception");
            }
            finally
            {
                ChannelList = null;
            }
        }

        /// <summary>
        /// Dispose Channels
        /// </summary>
        /// <param name="ChannelList"></param>
        /// <returns></returns>
        private static void DisposeChannels(List<ChannelResource> ChannelList)
        {
            try
            {
                for (int i = 0; i < ChannelList.Count; i++)
                {

                    try { ChannelList[i].Disconnect(); }
                    catch { }
                    try { ChannelList[i].Dispose(); }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Error disposing");
                    }
                }
                ChannelList.Clear();
            }
            catch (Exception ee)
            {
                Log.WriteException(ee, "DisposeChannels Exception");
            }
            finally
            {
                ChannelList = null;
            }
        }

        #endregion

    }

    #region Enumerations

    /// <summary>
    /// Enumeration channel type
    /// </summary>
    public enum ChannelType
    {
        MsiChannel,
        T1Channel,
        SipChannel,
        DigitalChannel
    }

    /// <summary>
    /// Enumeration channel status
    /// </summary>
    public enum ChannelStatus
    {
        Busy,
        Available
    }

    #endregion

}
