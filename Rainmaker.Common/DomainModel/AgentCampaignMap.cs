using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public class AgentCampaignMap
    {
        public AgentCampaignMap()
        {
            ResetAgentCampaignMap();
        }

        public void ResetAgentCampaignMap()
        {
            this.agentCampaignMapID = 0;
            this.agentID = 0;
            this.campaignID = 0;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long agentCampaignMapID;
        [XmlAttribute("AgentCampaignMapID")]
        public long AgentCampaignMapID
        {
            get { return this.agentCampaignMapID; }
            set { this.agentCampaignMapID = value; }
        }

        private long agentID;
        [XmlAttribute("AgentID")]
        public long AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
        }

        private long campaignID;
        [XmlAttribute("CampaignID")]
        public long CampaignID
        {
            get { return this.campaignID; }
            set { this.campaignID = value; }
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
