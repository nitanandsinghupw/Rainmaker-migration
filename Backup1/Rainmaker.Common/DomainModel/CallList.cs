using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("CallList")]
    public class CallList
    {
        public CallList()
        {
            ResetCallList();
        }

        public void ResetCallList()
        {
            this.callListID = 0;
            this.agentID = 0;
            this.resultCodeID = 0;
            this.phoneNumber = "";
            this.callDate = DateTime.Now;
            this.callTime = DateTime.Now;
            this.callDuration = 0;
            this.callCompletionTime = DateTime.Now;
            this.callWrapTime = DateTime.Now;
            this.isBlocked = false;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.queryID = 0;
        }

        private long callListID;
        [XmlAttribute("CallListID")]
        public long CallListID
        {
            get { return this.callListID; }
            set { this.callListID = value; }
        }
        
        private long agentID;
        [XmlAttribute("AgentID")]
        public long AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
        }

        private long resultCodeID;
        [XmlAttribute("ResultCodeID")]
        public long ResultCodeID
        {
            get { return this.resultCodeID; }
            set { this.resultCodeID = value; }
        }

        private string phoneNumber;
        [XmlAttribute("PhoneNumber")]
        public string PhoneNumber
        {
            get { return this.phoneNumber; }
            set { this.phoneNumber = value; }
        }

        private DateTime callDate;
        [XmlAttribute("CallDate")]
        public DateTime CallDate
        {
            get { return this.callDate; }
            set { this.callDate = value; }
        }

        private DateTime callTime;
        [XmlAttribute("CallTime")]
        public DateTime CallTime
        {
            get { return this.callTime; }
            set { this.callTime = value; }
        }

        private decimal callDuration;
        [XmlAttribute("CallDuration")]
        public decimal CallDuration
        {
            get { return this.callDuration; }
            set { this.callDuration = value; }
        }

        private DateTime callCompletionTime;
        [XmlAttribute("CallCompletionTime")]
        public DateTime CallCompletionTime
        {
            get { return this.callCompletionTime; }
            set { this.callCompletionTime = value; }
        }

        private DateTime callWrapTime;
        [XmlAttribute("CallWrapTime")]
        public DateTime CallWrapTime
        {
            get { return this.callWrapTime; }
            set { this.callWrapTime = value; }
        }

        private bool isBlocked;
        [XmlAttribute("IsBlocked")]
        public bool IsBlocked
        {
            get { return this.isBlocked; }
            set { this.isBlocked = value; }
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

        private long queryID;
        [XmlAttribute("QueryID")]
        public long QueryID
        {
            get { return this.queryID; }
            set { this.queryID = value; }
        }
    }
}
