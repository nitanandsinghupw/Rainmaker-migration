using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public enum DialingMode
    {
        OutboundOnly = 1,
        InboundOutbound = 2,
        InboundOnly = 3,
        PowerDial = 4,
        ManualDial = 5,
        Unmanned = 6,
    }
    public enum AnsweringMachineMode
    {
        Quick = 1,
        Complete = 2,
    }

    [Serializable()]
    [XmlRoot("DialingParameter")]
    public class DialingParameter
    {

        public DialingParameter()
        {
            ResetDialingParameter();
        }

        public void ResetDialingParameter()
        {
            this.dailingParameterID = 0;
            this.phoneLineCount = 0;
            this.dropRatePercent = 0;
            this.ringSeconds = 0;
            this.minimumDelayBetweenCalls = 0;
            this.dialingMode = 0;
            this.answeringMachineDetection = true;
            this.sevenDigitPrefix = "";
            this.tenDigitPrefix = "";
            this.coldCallScriptID = 0;
            this.verificationScriptID = 0;
            this.inboundScriptID = 0;
            this.amCallTimes = 0;
            this.pmCallTimes = 0;
            this.weekendCallTimes = 0;
            this.ansMachDetect = 0;
            this.amDialingStart = DateTime.Now;
            this.amDialingStop = DateTime.Now;
            this.pmDialingStart = DateTime.Now;
            this.pmDialingStop = DateTime.Now;
            this.sevenDigitSuffix = "";
            this.tenDigitSuffix = "";
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.errorRedialLapse = 0;
            this.busyRedialLapse = 0;
            this.noAnswerRedialLapse = 0;
            this.channelsPerAgent = 2;
            this.defaultCallLapse = 0;
            this.humanMessageEnable = false;
            this.silentCallMessageEnable = false;
            this.answeringMachineMessage = "";
            this.humanMessage = "";
            this.silentCallMessage = "";
            this.activeDialingAlgorithm = 1;
            this.dropRateThrottle = 1;
            this.callStatisticsWindow = 100;
        }

        private int ansMachDetect;
        [XmlAttribute("AnsMachDetect")]
        public int AnsMachDetect
        {
            get { return this.ansMachDetect; }
            set { this.ansMachDetect = value; }
        }

        private long dailingParameterID;
        [XmlAttribute("DailingParameterID")]
        public long DailingParameterID
        {
            get { return this.dailingParameterID; }
            set { this.dailingParameterID = value; }
        }
        private int phoneLineCount;
        [XmlAttribute("PhoneLineCount")]
        public int PhoneLineCount
        {
            get { return this.phoneLineCount; }
            set { this.phoneLineCount = value; }
        }

        private int dropRatePercent;
        [XmlAttribute("DropRatePercent")]
        public int DropRatePercent
        {
            get { return this.dropRatePercent; }
            set { this.dropRatePercent = value; }
        }

        private int ringSeconds;
        [XmlAttribute("RingSeconds")]
        public int RingSeconds
        {
            get { return this.ringSeconds; }
            set { this.ringSeconds = value; }
        }

        private int minimumDelayBetweenCalls;
        [XmlAttribute("MinimumDelayBetweenCalls")]
        public int MinimumDelayBetweenCalls
        {
            get { return this.minimumDelayBetweenCalls; }
            set { this.minimumDelayBetweenCalls = value; }
        }

        private int dialingMode;
        [XmlAttribute("DialingMode")]
        public int DialingMode
        {
            get { return this.dialingMode; }
            set { this.dialingMode = value; }
        }

        private bool answeringMachineDetection;
        [XmlAttribute("AnsweringMachineDetection")]
        public bool AnsweringMachineDetection
        {
            get { return this.answeringMachineDetection; }
            set { this.answeringMachineDetection = value; }
        }

        private string sevenDigitPrefix;
        [XmlAttribute("SevenDigitPrefix")]
        public string SevenDigitPrefix
        {
            get { return this.sevenDigitPrefix; }
            set { this.sevenDigitPrefix = value; }
        }

        private string tenDigitPrefix;
        [XmlAttribute("TenDigitPrefix")]
        public string TenDigitPrefix
        {
            get { return this.tenDigitPrefix; }
            set { this.tenDigitPrefix = value; }
        }

        private long coldCallScriptID;
        [XmlAttribute("ColdCallScriptID")]
        public long ColdCallScriptID
        {
            get { return this.coldCallScriptID; }
            set { this.coldCallScriptID = value; }
        }

        private long verificationScriptID;
        [XmlAttribute("VerificationScriptID")]
        public long VerificationScriptID
        {
            get { return this.verificationScriptID; }
            set { this.verificationScriptID = value; }
        }

        private long inboundScriptID;
        [XmlAttribute("InboundScriptID")]
        public long InboundScriptID
        {
            get { return this.inboundScriptID; }
            set { this.inboundScriptID = value; }
        }

        private int amCallTimes;
        [XmlAttribute("AMCallTimes")]
        public int AMCallTimes
        {
            get { return this.amCallTimes; }
            set { this.amCallTimes = value; }
        }

        private int pmCallTimes;
        [XmlAttribute("PMCallTimes")]
        public int PMCallTimes
        {
            get { return this.pmCallTimes; }
            set { this.pmCallTimes = value; }
        }

        private int weekendCallTimes;
        [XmlAttribute("WeekendCallTimes")]
        public int WeekendCallTimes
        {
            get { return this.weekendCallTimes; }
            set { this.weekendCallTimes = value; }
        }

        private DateTime amDialingStart;
        [XmlAttribute("AMDialingStart")]
        public DateTime AMDialingStart
        {
            get { return this.amDialingStart; }
            set { this.amDialingStart = value; }
        }

        private DateTime amDialingStop;
        [XmlAttribute("AMDialingStop")]
        public DateTime AMDialingStop
        {
            get { return this.amDialingStop; }
            set { this.amDialingStop = value; }
        }

        private DateTime pmDialingStart;
        [XmlAttribute("PMDialingStart")]
        public DateTime PMDialingStart
        {
            get { return this.pmDialingStart; }
            set { this.pmDialingStart = value; }
        }

        private DateTime pmDialingStop;
        [XmlAttribute("PMDialingStop")]
        public DateTime PMDialingStop
        {
            get { return this.pmDialingStop; }
            set { this.pmDialingStop = value; }
        }

        private string sevenDigitSuffix;
        [XmlAttribute("SevenDigitSuffix")]
        public string SevenDigitSuffix
        {
            get { return this.sevenDigitSuffix; }
            set { this.sevenDigitSuffix = value; }
        }

        private string tenDigitSuffix;
        [XmlAttribute("TenDigitSuffix")]
        public string TenDigitSuffix
        {
            get { return this.tenDigitSuffix; }
            set { this.tenDigitSuffix = value; }
        }

        private DateTime dateCreated;
        [XmlAttribute("DateCreated")]
        public DateTime DateCreated
        {
            get { return this.dateCreated; }
            set { this.dateCreated = value; }
        }

        private DateTime dateModified;
        [XmlAttribute("DateModified")]
        public DateTime DateModified
        {
            get { return this.dateModified; }
            set { this.dateModified = value; }
        }

        private int errorRedialLapse;
        [XmlAttribute("ErrorRedialLapse")]
        public int ErrorRedialLapse
        {
            get { return this.errorRedialLapse; }
            set { this.errorRedialLapse = value; }
        }

        private int busyRedialLapse;
        [XmlAttribute("BusyRedialLapse")]
        public int BusyRedialLapse
        {
            get { return this.busyRedialLapse; }
            set { this.busyRedialLapse = value; }
        }

        private int noAnswerRedialLapse;
        [XmlAttribute("NoAnswerRedialLapse")]
        public int NoAnswerRedialLapse
        {
            get { return this.noAnswerRedialLapse; }
            set { this.noAnswerRedialLapse = value; }
        }

        private decimal channelsPerAgent;
        [XmlAttribute("ChannelsPerAgent")]
        public decimal ChannelsPerAgent
        {
            get { return this.channelsPerAgent; }
            set { this.channelsPerAgent = value; }
        }

        private int defaultCallLapse;
        [XmlAttribute("DefaultCallLapse")]
        public int DefaultCallLapse
        {
            get { return this.defaultCallLapse; }
            set { this.defaultCallLapse = value; }
        }

        private bool humanMessageEnable = false;
        [XmlAttribute("HumanMessageEnable")]
        public bool HumanMessageEnable
        {
            get { return this.humanMessageEnable; }
            set { this.humanMessageEnable = value; }
        }

        private string humanMessage;
        [XmlAttribute("HumanMessage")]
        public string HumanMessage
        {
            get { return this.humanMessage; }
            set { this.humanMessage = value; }
        }

        private bool silentCallMessageEnable = false;
        [XmlAttribute("SilentCallMessageEnable")]
        public bool SilentCallMessageEnable
        {
            get { return this.silentCallMessageEnable; }
            set { this.silentCallMessageEnable = value; }
        }

        private string silentCallMessage;
        [XmlAttribute("SilentCallMessage")]
        public string SilentCallMessage
        {
            get { return this.silentCallMessage; }
            set { this.silentCallMessage = value; }
        }

        private string answeringMachineMessage;
        [XmlAttribute("AnsweringMachineMessage")]
        public string AnsweringMachineMessage
        {
            get { return this.answeringMachineMessage; }
            set { this.answeringMachineMessage = value; }
        }

        private int activeDialingAlgorithm;
        [XmlAttribute("ActiveDialingAlgorithm")]
        public int ActiveDialingAlgorithm
        {
            get { return this.activeDialingAlgorithm; }
            set { this.activeDialingAlgorithm = value; }
        }

        private int callStatisticsWindow;
        [XmlAttribute("CallStatisticsWindow")]
        public int CallStatisticsWindow
        {
            get { return this.callStatisticsWindow; }
            set { this.callStatisticsWindow = value; }
        }

        private decimal dropRateThrottle;
        [XmlAttribute("DropRateThrottle")]
        public decimal DropRateThrottle
        {
            get { return this.dropRateThrottle; }
            set { this.dropRateThrottle = value; }
        }

        private int dropRateSensitivity;
        [XmlAttribute("DropRateSensitivity")]
        public int DropRateSensitivity
        {
            get { return this.dropRateSensitivity; }
            set { this.dropRateSensitivity = value; }
        }
    }

    public class tConnect
    {
        public string SecU = "69aa1D4682b04E93bF2309b5264F916b";
        public string SecP = "10082b0B2e524C49aE3717b5C1899Cd3";
    }
}
