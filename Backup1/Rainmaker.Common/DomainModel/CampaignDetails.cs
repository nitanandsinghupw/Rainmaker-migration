using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("CampaignDetails")]
    public class CampaignDetails
    {
        private long uniqueKey;
        private int campaign = -1;
        private string phoneNum;
        private string dBCompany;
        private int neverCallFlag = -1;
        private string agentID;
        private string agentName;
        private string verificationAgentID;
        private string offsiteTransferNumber = "";
        private int callResultCode = -1;
        private DateTime dateTimeofCall = DateTime.MinValue;
        private string callDuration;
        private DateTime callSenttoDialTime = DateTime.MinValue;
        private DateTime calltoAgentTime = DateTime.MinValue;
        private DateTime callHangupTime = DateTime.MinValue;
        private DateTime callCompletionTime = DateTime.MinValue;
        private DateTime callWrapUpStartTime = DateTime.MinValue;
        private DateTime callWrapUpStopTime = DateTime.MinValue;
        private DateTime resultCodeSetTime = DateTime.MinValue;
        private string totalNumAttempts;
        private string numAttemptsAM;
        private string numAttemptsWkEnd;
        private string numAttemptsPM;
        private string leadProcessed;
        private int fullQueryPassCount = -1;
        private string aPCRAgent;
        private string aPCRDT;
        private string aPCR;
        private string aPCRMemo;
        private string aPCR2;
        private string aPCRAgent2;
        private string aPCRDT2;
        private string aPCRMemo2;
        private string aPCR3;
        private string aPCRAgent3;
        private string aPCRDT3;
        private string aPCRMemo3;
        private string aPCR4;
        private string aPCRAgent4;
        private string aPCRDT4;
        private string aPCRMemo4;
        private string aPCR5;
        private string aPCRAgent5;
        private string aPCRDT5;
        private string aPCRMemo5;
        private string aPCR6;
        private string aPCRAgent6;
        private string aPCRDT6;
        private string aPCRMemo6;
        private DateTime scheduleDate;
        private string scheduleNotes = "";
        private long queryId = 0;
        private int orderIndex = 1;

        [XmlAttribute("UniqueKey")]
        public long UniqueKey
        {
            get { return this.uniqueKey; }
            set { this.uniqueKey = value; }
        }

        [XmlAttribute("Campaign")]
        public int Campaign
        {
            get { return this.campaign; }
            set { this.campaign = value; }
        }

        [XmlAttribute("PhoneNum")]
        public string PhoneNum
        {
            get { return this.phoneNum; }
            set { this.phoneNum = this.Format(value); }
        }

        private string Format(string value)
        {
            try
            {
                return value.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            }
            catch { return value; }
        }


        [XmlAttribute("DBCompany")]
        public string DBCompany
        {
            get { return this.dBCompany; }
            set { this.dBCompany = value; }
        }


        [XmlAttribute("NeverCallFlag")]
        public int NeverCallFlag
        {
            get { return this.neverCallFlag; }
            set { this.neverCallFlag = value; }
        }

        [XmlAttribute("AgentID")]
        public string AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
        }

        [XmlAttribute("AgentName")]
        public string AgentName
        {
            get { return this.agentName; }
            set { this.agentName = value; }
        }

        [XmlAttribute("VerificationAgentID")]
        public string VerificationAgentID
        {
            get { return this.verificationAgentID; }
            set { this.verificationAgentID = value; }
        }

        [XmlAttribute("OffsiteTransferNumber")]
        public string OffsiteTransferNumber
        {
            get { return this.offsiteTransferNumber; }
            set { this.offsiteTransferNumber = value; }
        }

        [XmlAttribute("CallResultCode")]
        public int CallResultCode
        {
            get { return this.callResultCode; }
            set { this.callResultCode = value; }
        }

        [XmlAttribute("DateTimeofCall")]
        public DateTime DateTimeofCall
        {
            get { return this.dateTimeofCall; }
            set { this.dateTimeofCall = value; }
        }

        [XmlAttribute("CallDuration")]
        public string CallDuration
        {
            get { return this.callDuration; }
            set { this.callDuration = value; }
        }

        [XmlAttribute("CallSenttoDialTime")]
        public DateTime CallSenttoDialTime
        {
            get { return this.callSenttoDialTime; }
            set { this.callSenttoDialTime = value; }
        }

        [XmlAttribute("CalltoAgentTime")]
        public DateTime CalltoAgentTime
        {
            get { return this.calltoAgentTime; }
            set { this.calltoAgentTime = value; }
        }

        [XmlAttribute("CallHangupTime")]
        public DateTime CallHangupTime
        {
            get { return this.callHangupTime; }
            set { this.callHangupTime = value; }
        }

        [XmlAttribute("CallCompletionTime")]
        public DateTime CallCompletionTime
        {
            get { return this.callCompletionTime; }
            set { this.callCompletionTime = value; }
        }

        [XmlAttribute("CallWrapUpStartTime")]
        public DateTime CallWrapUpStartTime
        {
            get { return this.callWrapUpStartTime; }
            set { this.callWrapUpStartTime = value; }
        }

        [XmlAttribute("CallWrapUpStopTime")]
        public DateTime CallWrapUpStopTime
        {
            get { return this.callWrapUpStopTime; }
            set { this.callWrapUpStopTime = value; }
        }

        [XmlAttribute("ResultCodeSetTime")]
        public DateTime ResultCodeSetTime
        {
            get { return this.resultCodeSetTime; }
            set { this.resultCodeSetTime = value; }
        }

        [XmlAttribute("TotalNumAttempts")]
        public string TotalNumAttempts
        {
            get { return this.totalNumAttempts; }
            set { this.totalNumAttempts = value; }
        }

        [XmlAttribute("NumAttemptsAM")]
        public string NumAttemptsAM
        {
            get { return this.numAttemptsAM; }
            set { this.numAttemptsAM = value; }
        }

        [XmlAttribute("NumAttemptsWkEnd")]
        public string NumAttemptsWkEnd
        {
            get { return this.numAttemptsWkEnd; }
            set { this.numAttemptsWkEnd = value; }
        }

        [XmlAttribute("NumAttemptsPM")]
        public string NumAttemptsPM
        {
            get { return this.numAttemptsPM; }
            set { this.numAttemptsPM = value; }
        }

        [XmlAttribute("LeadProcessed")]
        public string LeadProcessed
        {
            get { return this.leadProcessed; }
            set { this.leadProcessed = value; }
        }
        // *** removed per change from Name to First Last.  Seems to be unused
        //[XmlAttribute("NAME")]
        //public string NAME
        //{
        //    get { return this.nAME; }
        //    set { this.nAME = value; }
        //}

        //[XmlAttribute("ADDRESS")]
        //public string ADDRESS
        //{
        //    get { return this.aDDRESS; }
        //    set { this.aDDRESS = value; }
        //}

        //[XmlAttribute("CITY")]
        //public string CITY
        //{
        //    get { return this.cITY; }
        //    set { this.cITY = value; }
        //}

        //[XmlAttribute("STATE")]
        //public string STATE
        //{
        //    get { return this.sTATE; }
        //    set { this.sTATE = value; }
        //}

        //[XmlAttribute("ZIP")]
        //public string ZIP
        //{
        //    get { return this.zIP; }
        //    set { this.zIP = value; }
        //}

        //[XmlAttribute("ADDRESS2")]
        //public string ADDRESS2
        //{
        //    get { return this.aDDRESS2; }
        //    set { this.aDDRESS2 = value; }
        //}

        //[XmlAttribute("COUNTRY")]
        //public string COUNTRY
        //{
        //    get { return this.cOUNTRY; }
        //    set { this.cOUNTRY = value; }
        //}

        [XmlAttribute("FullQueryPassCount")]
        public int FullQueryPassCount
        {
            get { return this.fullQueryPassCount; }
            set { this.fullQueryPassCount = value; }
        }

        [XmlAttribute("APCR")]
        public string APCR
        {
            get { return this.aPCR; }
            set { this.aPCR = value; }
        }

        [XmlAttribute("APCRAgent")]
        public string APCRAgent
        {
            get { return this.aPCRAgent; }
            set { this.aPCRAgent = value; }
        }

        [XmlAttribute("APCRDT")]
        public string APCRDT
        {
            get { return this.aPCRDT; }
            set { this.aPCRDT = value; }
        }

        [XmlAttribute("APCRMemo")]
        public string APCRMemo { get { return this.aPCRMemo; } set { this.aPCRMemo = value; } }

        [XmlAttribute("APCR2")]
        public string APCR2 { get { return this.aPCR2; } set { this.aPCR2 = value; } }

        [XmlAttribute("APCRAgent2")]
        public string APCRAgent2 { get { return this.aPCRAgent2; } set { this.aPCRAgent2 = value; } }

        [XmlAttribute("APCRDT2")]
        public string APCRDT2 { get { return this.aPCRDT2; } set { this.aPCRDT2 = value; } }

        [XmlAttribute("APCRMemo2")]
        public string APCRMemo2 { get { return this.aPCRMemo2; } set { this.aPCRMemo2 = value; } }

        [XmlAttribute("APCR3")]
        public string APCR3 { get { return this.aPCR3; } set { this.aPCR3 = value; } }

        [XmlAttribute("APCRAgent3")]
        public string APCRAgent3 { get { return this.aPCRAgent3; } set { this.aPCRAgent3 = value; } }

        [XmlAttribute("APCRDT3")]
        public string APCRDT3 { get { return this.aPCRDT3; } set { this.aPCRDT3 = value; } }

        [XmlAttribute("APCRMemo3")]
        public string APCRMemo3 { get { return this.aPCRMemo3; } set { this.aPCRMemo3 = value; } }

        [XmlAttribute("APCR4")]
        public string APCR4 { get { return this.aPCR4; } set { this.aPCR4 = value; } }

        [XmlAttribute("APCRAgent4")]
        public string APCRAgent4 { get { return this.aPCRAgent4; } set { this.aPCRAgent4 = value; } }

        [XmlAttribute("APCRDT4")]
        public string APCRDT4 { get { return this.aPCRDT4; } set { this.aPCRDT4 = value; } }

        [XmlAttribute("APCRMemo4")]
        public string APCRMemo4 { get { return this.aPCRMemo4; } set { this.aPCRMemo4 = value; } }

        [XmlAttribute("APCR5")]
        public string APCR5
        {
            get { return this.aPCR5; }
            set { this.aPCR5 = value; }
        }

        [XmlAttribute("APCRAgent5")]
        public string APCRAgent5
        {
            get { return this.aPCRAgent5; }
            set { this.aPCRAgent5 = value; }
        }

        [XmlAttribute("APCRDT5")]
        public string APCRDT5
        {
            get { return this.aPCRDT5; }
            set { this.aPCRDT5 = value; }
        }

        [XmlAttribute("APCRMemo5")]
        public string APCRMemo5
        {
            get { return this.aPCRMemo5; }
            set { this.aPCRMemo5 = value; }
        }

        [XmlAttribute("APCR6")]
        public string APCR6
        {
            get { return this.aPCR6; }
            set { this.aPCR6 = value; }
        }

        [XmlAttribute("APCRAgent6")]
        public string APCRAgent6
        {
            get { return this.aPCRAgent6; }
            set { this.aPCRAgent6 = value; }
        }

        [XmlAttribute("APCRDT6")]
        public string APCRDT6
        {
            get { return this.aPCRDT6; }
            set { this.aPCRDT6 = value; }
        }

        [XmlAttribute("APCRMemo6")]
        public string APCRMemo6
        {
            get { return this.aPCRMemo6; }
            set { this.aPCRMemo6 = value; }
        }

        [XmlAttribute("ScheduleDate")]
        public DateTime ScheduleDate
        {
            get { return this.scheduleDate; }
            set { this.scheduleDate = value; }
        }

        [XmlAttribute("ScheduleNotes")]
        public string ScheduleNotes
        {
            get { return this.scheduleNotes; }
            set { this.scheduleNotes = value; }
        }

        [XmlAttribute("QueryId")]
        public long QueryId
        {
            get { return this.queryId; }
            set { this.queryId = value; }
        }

        [XmlAttribute("OrderIndex")]
        public int OrderIndex
        {
            get { return this.orderIndex; }
            set { this.orderIndex = value; }
        }

    }

    public enum CallType
    {
        AMCall = 0,
        PMCall = 1,
        WkendCall = 2
    }
}
