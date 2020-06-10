using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public enum AgentStatus
    {
        Authenticated,
        UserDoesNotExist,
        IncorrectPassword,
        Available,
        Busy,
        Paused,
        NoStatus
    }

    [Serializable()]
    [XmlRoot("Agent")]
    public class Agent
    {
        private long agentID;
        private string agentName;
        private string loginName;
        private string password;
        private bool isAdministrator;
        private bool allowManualDial;
        private bool verificationAgent;
        private bool inBoundAgent;
        private string phoneNumber;
        private string shortDescription;
        private long agentActivityID;
        private long agentStatusID;
        private long receiptModeID;
        private long campaignID;
        private DateTime loginTime;
        private DateTime logoutTime;
        private DateTime dateCreated;
        private DateTime dateModified;
        private AgentStatus status;
        private bool isDeleted;
        private string outboundCallerID;
        private string stationNumber;
        private bool allwaysOffHook;
        private string campaignDB;
        //private AgentActivity agentActivity;
        private string stationHost;
        private bool pauseInformed;
        private bool idleInformed;
        private bool phoneInformed;
        private bool isReset;
        private long callUniqueKey;

        public Agent()
        {
            ResetAgent();
        }
        
        public void ResetAgent()
        {
            this.agentID = 0;
            this.agentName = "";
            this.loginName = "";
            this.password = "";
            this.phoneNumber = "";
            this.shortDescription = "";
            this.isAdministrator = false;
            this.allowManualDial = false;
            this.verificationAgent = false;
            this.inBoundAgent = false;
            this.isDeleted = false;
            this.agentActivityID = 0;
            this.campaignID = 0;
            this.agentStatusID = 0;
            this.receiptModeID = 0;
            this.loginTime = DateTime.Now;
            this.logoutTime = DateTime.Now;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.status = AgentStatus.NoStatus;
            this.outboundCallerID = "";
            this.stationNumber = "";
            this.allwaysOffHook = false;
            this.campaignDB = "";
            this.pauseInformed = false;
            this.idleInformed = false;
            this.phoneInformed = false;
            this.isReset = false;
            this.callUniqueKey = 0;
        }

        [XmlAttribute("AgentID")]
        public long AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
        }

        [XmlAttribute("IsDeleted")]
        public bool IsDeleted
        {
            get { return this.isDeleted; }
            set { this.isDeleted = value; }
        }

        [XmlAttribute("IsReset")]
        public bool IsReset
        {
            get { return this.isReset; }
            set { this.isReset = value; }
        }
        
        [XmlAttribute("AgentActivityID")]
        public long AgentActivityID
        {
            get { return this.agentActivityID; }
            set { this.agentActivityID = value; }
        }

        [XmlAttribute("AgentStatusID")]
        public long AgentStatusID
        {
            get { return this.agentStatusID; }
            set { this.agentStatusID = value; }
        }

        
        [XmlAttribute("ReceiptModeID")]
        public long ReceiptModeID
        {
            get { return this.receiptModeID; }
            set { this.receiptModeID = value; }
        }

        [XmlAttribute("CampaignID")]
        public long CampaignID
        {
            get { return this.campaignID; }
            set { this.campaignID = value; }
        }

        [XmlAttribute("CampaignDB")]
        public string CampaignDB
        {
            get { return this.campaignDB; }
            set { this.campaignDB = value; }
        }

        [XmlAttribute("LoginTime")]
        public DateTime LoginTime
        {
            get { return this.loginTime; }
            set { this.loginTime = value; }
        }

        [XmlAttribute("LogoutTime")]
        public DateTime LogoutTime
        {
            get { return this.logoutTime; }
            set { this.logoutTime = value; }
        }

        [XmlAttribute("AgentName")]
        public string AgentName
        {
            get { return this.agentName; }
            set { this.agentName = value; }
        }

        [XmlAttribute("LoginName")]
        public string LoginName
        {
            get { return this.loginName; }
            set { this.loginName = value; }
        }

        [XmlAttribute("Password")]
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        [XmlAttribute("IsAdministrator")]
        public bool IsAdministrator
        {
            get { return this.isAdministrator; }
            set { this.isAdministrator = value; }
        }

        [XmlAttribute("AllowManualDial")]
        public bool AllowManualDial
        {
            get { return this.allowManualDial; }
            set { this.allowManualDial = value; }
        }

        [XmlAttribute("VerificationAgent")]
        public bool VerificationAgent
        {
            get { return this.verificationAgent; }
            set { this.verificationAgent = value; }
        }

        [XmlAttribute("InBoundAgent")]
        public bool InBoundAgent
        {
            get { return this.inBoundAgent; }
            set { this.inBoundAgent = value; }
        }

        [XmlAttribute("PhoneNumber")]
        public string PhoneNumber
        {
            get { return this.phoneNumber; }
            set { this.phoneNumber = value; }
        }
        [XmlAttribute("ShortDescription")]
        public string ShortDescription
        {
            get { return this.shortDescription; }
            set { this.shortDescription = value; }
        }

        [XmlAttribute("DateCreated")]
        public DateTime DateCreated
        {
            get { return this.dateCreated; }
            set { this.dateCreated = value; }
        }

        [XmlAttribute("DateModified")]
        public DateTime DateModified
        {
            get { return this.dateModified; }
            set { this.dateModified = value; }
        }

        [XmlAttribute("Status")]
        public AgentStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        [XmlAttribute("OutboundCallerID")]
        public string OutboundCallerID
        {
            get { return this.outboundCallerID; }
            set { this.outboundCallerID = value; }
        }

        [XmlAttribute("StationNumber")]
        public string StationNumber
        {
            get { return this.stationNumber; }
            set { this.stationNumber = value; }
        }

        [XmlAttribute("AllwaysOffHook")]
        public bool AllwaysOffHook
        {
            get { return this.allwaysOffHook; }
            set { this.allwaysOffHook = value; }
        }


        [XmlAttribute("StationHost")]
        public string StationHost
        {
            get { return this.stationHost; }
            set { this.stationHost = value; }
        }

        [XmlAttribute("PauseInformed")]
        public bool PauseInformed
        {
            get { return this.pauseInformed; }
            set { this.pauseInformed = value; }
        }

        [XmlAttribute("IdleInformed")]
        public bool IdleInformed
        {
            get { return this.idleInformed; }
            set { this.idleInformed = value; }
        }

        [XmlAttribute("NoPhoneInformed")]
        public bool PhoneInformed
        {
            get { return this.phoneInformed; }
            set { this.phoneInformed = value; }
        }

        [XmlAttribute("CallUniqueKey")]
        public long CallUniqueKey
        {
            get { return this.callUniqueKey; }
            set { this.callUniqueKey = value; }
        }
    }    
}
