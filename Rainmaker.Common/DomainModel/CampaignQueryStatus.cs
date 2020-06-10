using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("CampaignQueryStatus")]
    public class CampaignQueryStatus
    {
        public CampaignQueryStatus()
        {
            ResetCampaignQueryStatus();
        }

        public void ResetCampaignQueryStatus()
        {
            this.campaignQueryID = 0;
            this.queryID = 0;
            this.queryName = "";
            this.isActive = true;
            this.isStandby = false;
            this.total = 0;
            this.available = 0;
            this.dials = 0;
            this.talks = 0;
            this.answeringMachine = 0;
            this.noAnswer = 0;
            this.busy = 0;
            this.opInt = 0;
            this.drops = 0;
            this.failed = 0;
            this.resultCodeId = 0;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.isCurrent = false;
        }

        private long campaignQueryID;
        [XmlAttribute("CampaignQueryID")]
        public long CampaignQueryID
        {
            get { return this.campaignQueryID; }
            set { this.campaignQueryID = value; }
        }

        private long queryID;
        [XmlAttribute("QueryID")]
        public long QueryID
        {
            get { return this.queryID; }
            set { this.queryID = value; }
        }

        private string queryName;
        [XmlAttribute("QueryName")]
        public string QueryName
        {
            get { return this.queryName; }
            set { this.queryName = value; }
        }

        private bool isActive;
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }

        private bool isStandby;
        [XmlAttribute("IsStandby")]
        public bool IsStandby
        {
            get { return this.isStandby; }
            set { this.isStandby = value; }
        }

        private int total;
        [XmlAttribute("Total")]
        public int Total
        {
            get { return this.total; }
            set { this.total = value; }
        }

        private int available;
        [XmlAttribute("Available")]
        public int Available
        {
            get { return this.available; }
            set { this.available = value; }
        }

        private int dials;
        [XmlAttribute("Dials")]
        public int Dials
        {
            get { return this.dials; }
            set { this.dials = value; }
        }

        private int talks;
        [XmlAttribute("Talks")]
        public int Talks
        {
            get { return this.talks; }
            set { this.talks = value; }
        }

        private int answeringMachine;
        [XmlAttribute("AnsweringMachine")]
        public int AnsweringMachine
        {
            get { return this.answeringMachine; }
            set { this.answeringMachine = value; }
        }

        private int noAnswer;
        [XmlAttribute("NoAnswer")]
        public int NoAnswer
        {
            get { return this.noAnswer; }
            set { this.noAnswer = value; }
        }

        private int busy;
        [XmlAttribute("Busy")]
        public int Busy
        {
            get { return this.busy; }
            set { this.busy = value; }
        }

        private int opInt;
        [XmlAttribute("OpInt")]
        public int OpInt
        {
            get { return this.opInt; }
            set { this.opInt = value; }
        }

        private int drops;
        [XmlAttribute("Drops")]
        public int Drops
        {
            get { return this.drops; }
            set { this.drops = value; }
        }

        private int failed;
        [XmlAttribute("Failed")]
        public int Failed
        {
            get { return this.failed; }
            set { this.failed = value; }
        }

        private int resultCodeId;
        [XmlAttribute("ResultCodeId")]
        public int ResultCodeId
        {
            get { return this.resultCodeId; }
            set { this.resultCodeId = value; }
        }

        [XmlAttribute("DropsPercentage")]
        public decimal DropsPercentage
        {
            get { return (this.drops * this.total)/100; }            
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

        private bool isCurrent;
        [XmlAttribute("IsCurrent")]
        public bool IsCurrent
        {
            get { return this.isCurrent; }
            set { this.isCurrent = value; }
        }

    }
}
