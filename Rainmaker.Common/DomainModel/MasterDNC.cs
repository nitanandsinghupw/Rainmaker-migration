using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{

    [Serializable()]
    [XmlRoot("MasterDNC")]
    public class MasterDNC
    {
        private long uniqueKey;
        private string campaignDBConnString;
        private string phoneNum;
        private string dbCompany;
		private string agentName;
	    private string agentID;
        private DateTime dateCreated;
        private DateTime dateModified;
        

        public MasterDNC()
        {
            ResetMasterDNC();
        }
        public void ResetMasterDNC()
        {
			this.uniqueKey = 0;
            this.campaignDBConnString = "";
			this.phoneNum = "";
			this.dbCompany = "";
			this.agentName = "";
			this.agentID = "";
			this.dateCreated = DateTime.Now;
			this.dateModified = DateTime.Now;
	
        }

        [XmlAttribute("uniqueKey")]
        public long UniqueKey
        {
            get { return this.uniqueKey; }
            set { this.uniqueKey = value; }
        }

        
        [XmlAttribute("CampaignDBConnString")]
        public string CampaignDBConnString
        {
            get { return this.campaignDBConnString; }
            set { this.campaignDBConnString = value; }
        }

        [XmlAttribute("PhoneNum")]
        public string PhoneNum
        {
            get { return this.phoneNum; }
            set { this.phoneNum = value; }
        }

        [XmlAttribute("DbCompany")]
        public string DbCompany
        {
            get { return this.dbCompany; }
            set { this.dbCompany = value; }
        }

        [XmlAttribute("AgentName")]
        public string AgentName
        {
            get { return this.agentName; }
            set { this.agentName = value; }
        }

        [XmlAttribute("AgentID")]
        public string AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
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

    }
}
