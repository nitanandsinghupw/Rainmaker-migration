using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
   
    public class AgentActivity
    {
        public AgentActivity()
        {
            ResetAgentActivity();
        }

        public void ResetAgentActivity()
        {
            this.agentActivityID = 0;
            this.agentID = 0;
            this.agentActivityID = 0;
            this.campaignID = 0;
            this.loginTime = DateTime.Now;
            this.logoutTime = DateTime.Now;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long agentActivityID;
        [XmlAttribute("AgentActivityID")]
        public long AgentActivityID
        {
            get { return this.agentActivityID; }
            set { this.agentActivityID = value; }
        }

        private long agentID;
        [XmlAttribute("AgentID")]
        public long AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
        }

        private long agentStatusID;
        [XmlAttribute("AgentStatusID")]
        public long AgentStatusID
        {
            get { return this.agentStatusID; }
            set { this.agentStatusID = value; }
        }

        private long campaignID;
        [XmlAttribute("CampaignID")]
        public long CampaignID
        {
            get { return this.campaignID; }
            set { this.campaignID = value; }
        }

        private DateTime loginTime;
        [XmlAttribute("LoginTime")]
        public DateTime LoginTime
        {
            get { return this.loginTime; }
            set { this.loginTime = value; }
        }

        private DateTime logoutTime;
        [XmlAttribute("LogoutTime")]
        public DateTime LogoutTime
        {
            get { return this.logoutTime; }
            set { this.logoutTime = value; }
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
    }
}
