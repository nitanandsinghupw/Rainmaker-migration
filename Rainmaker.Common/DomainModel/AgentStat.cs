                                                                                                                                                                                                                                                                          using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public enum agentStat
    {
        Waiting = 1,
        Talking = 2,
        Paused = 3,
        WrappingUp = 4
    }

    [Serializable()]
    [XmlRoot("AgentStat")]
    public class AgentStat
    {
        public AgentStat()
        {
            ResetAgentStat();
        }

        public void ResetAgentStat()
        {
            this.statID = 0;
            this.agentID = 0;
            this.statusID = 0;
            this.receiptModeID = 0;
            this.leadsSales = 0;
            this.pledgeAmount = 0;
            this.presentations = 0;
            this.calls = 0;
            this.LeadSalesRatio = 0;
            this.talkTime = 0;
            this.waitingTime = 0;
            this.pauseTime = 0;
            this.wrapTime = 0;
            this.loginDate = DateTime.MinValue;
            this.logOffDate = DateTime.MinValue;
            this.loginTime = DateTime.MinValue;
            this.LogOffTime = DateTime.MinValue;
            this.lastResultCodeID = 0;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.timeModified = DateTime.Now;
        }

        private long statID;
        [XmlAttribute("StatID")]
        public long StatID
        {
            get { return this.statID; }
            set { this.statID = value; }
        }

        private long agentID;
        [XmlAttribute("AgentID")]
        public long AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
        }

        private long statusID;
        [XmlAttribute("StatusID")]
        public long StatusID
        {
            get { return this.statusID; }
            set { this.statusID = value; }
        }

        private long receiptModeID;
        [XmlAttribute("ReceiptModeID")]
        public long ReceiptModeID
        {
            get { return this.receiptModeID; }
            set { this.receiptModeID = value; }
        }

        private int leadsSales;
        [XmlAttribute("LeadsSales")]
        public int LeadsSales
        {
            get { return this.leadsSales; }
            set { this.leadsSales = value; }
        }

        private int presentations;
        [XmlAttribute("Presentations")]
        public int Presentations
        {
            get { return this.presentations; }
            set { this.presentations = value; }
        }

        private int calls;
        [XmlAttribute("Calls")]
        public int Calls
        {
            get { return this.calls; }
            set { this.calls = value; }
        }

        private decimal leadSalesRatio;
        [XmlAttribute("LeadSalesRatio")]
        public decimal LeadSalesRatio
        {
            get { return this.leadSalesRatio; }
            set { this.leadSalesRatio = value; }
        }

        private decimal pledgeAmount;
        [XmlAttribute("PledgeAmount")]
        public decimal PledgeAmount
        {
            get { return this.pledgeAmount; }
            set { this.pledgeAmount = value; }
        }

        private decimal talkTime;
        [XmlAttribute("TalkTime")]
        public decimal TalkTime
        {
            get { return this.talkTime; }
            set { this.talkTime = value; }
        }

        private decimal waitingTime;
        [XmlAttribute("WaitingTime")]
        public decimal WaitingTime
        {
            get { return this.waitingTime; }
            set { this.waitingTime = value; }
        }

        private decimal pauseTime;
        [XmlAttribute("PauseTime")]
        public decimal PauseTime
        {
            get { return this.pauseTime; }
            set { this.pauseTime = value; }
        }

        private decimal wrapTime;
        [XmlAttribute("WrapTime")]
        public decimal WrapTime
        {
            get { return this.wrapTime; }
            set { this.wrapTime = value; }
        }

        private DateTime loginDate;
        [XmlAttribute("LoginDate")]
        public DateTime LoginDate
        {
            get { return this.loginDate; }
            set { this.loginDate = value; }
        }

        private DateTime loginTime;
        [XmlAttribute("LoginTime")]
        public DateTime LoginTime
        {
            get { return this.loginTime; }
            set { this.loginTime = value; }
        }

        private DateTime logOffDate;
        [XmlAttribute("LogOffDate")]
        public DateTime LogOffDate
        {
            get { return this.logOffDate; }
            set { this.logOffDate = value; }
        }

        private DateTime logOffTime;
        [XmlAttribute("LogOffTime")]
        public DateTime LogOffTime
        {
            get { return this.logOffTime; }
            set { this.logOffTime = value; }
        }

        private long lastResultCodeID;
        [XmlAttribute("LastResultCodeID")]
        public long LastResultCodeID
        {
            get { return this.lastResultCodeID; }
            set { this.lastResultCodeID = value; }
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

        private DateTime timeModified;
        [XmlAttribute("TimeModified")]
        public DateTime TimeModified
        {
            get { return this.timeModified; }
            set { this.timeModified = value; }
        }

        private string agentName;
        [XmlAttribute("AgentName")]
        public string AgentName
        {
            get { return this.agentName; }
            set { this.agentName = value; }
        }

        private string status;
        [XmlAttribute("Status")]
        public string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
    }
}
