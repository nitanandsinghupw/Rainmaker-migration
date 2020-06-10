using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public enum CampaignStatus
    {
        Idle = 1,
        Run = 2,
        Pause = 3,
        Completed = 4,
        FlushIdle = 5,
        FlushPaused = 6
    }

    [Serializable()]
    [XmlRoot("Campaign")]
    public class Campaign
    {
        private long campaignID;
        private long statusID;
        private string description;
        private string shortDescription;
        private string campaignDBConnString;
        private bool fundRaiserDataTracking;
        private bool recordLevelCallHistory;
        private bool onsiteTransfer;
        private bool enableAgentTraining;
        private bool isDeleted;
        private bool allowDuplicatePhones;
        private bool allow7DigitNums;
        private bool allow10DigitNums;
        private string duplicateRule;
        private bool flushCallQueueOnIdle;
        private bool dialAllNumbers;
        private DateTime dateCreated;
        private DateTime dateModified;
        private string outboundCallerID;
        private DateTime startTime = DateTime.MinValue;
        private DateTime stopTime = DateTime.MinValue;

        public Campaign()
        {
            ResetCampaign();
        }
        public void ResetCampaign()
        {
            this.campaignID = 0;
            this.statusID = 0;
            this.description = "";
            this.shortDescription = "";
            this.campaignDBConnString = "";
            this.fundRaiserDataTracking = false;
            this.recordLevelCallHistory = false;
            this.onsiteTransfer = false;
            this.enableAgentTraining = false;
            this.allowDuplicatePhones = false;
            this.allow10DigitNums = true;
            this.allow7DigitNums = false;
            this.duplicateRule = "I";
            this.isDeleted = false;
            this.flushCallQueueOnIdle = false;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.dialAllNumbers = false;
            this.outboundCallerID = "";
        }

        [XmlAttribute("CampaignID")]
        public long CampaignID
        {
            get { return this.campaignID; }
            set { this.campaignID = value; }
        }
        [XmlAttribute("StatusID")]
        public long StatusID
        {
            get { return this.statusID; }
            set { this.statusID = value; }
        }
        [XmlAttribute("IsDeleted")]
        public bool IsDeleted
        {
            get { return this.isDeleted; }
            set { this.isDeleted = value; }
        }
        [XmlAttribute("Description")]
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        [XmlAttribute("ShortDescription")]
        public string ShortDescription
        {
            get { return this.shortDescription; }
            set { this.shortDescription = value; }
        }

        [XmlAttribute("CampaignDBConnString")]
        public string CampaignDBConnString
        {
            get { return this.campaignDBConnString; }
            set { this.campaignDBConnString = value; }
        }

        [XmlAttribute("FundRaiserDataTracking")]
        public bool FundRaiserDataTracking
        {
            get { return this.fundRaiserDataTracking; }
            set { this.fundRaiserDataTracking = value; }
        }

        [XmlAttribute("RecordLevelCallHistory")]
        public bool RecordLevelCallHistory
        {
            get { return this.recordLevelCallHistory; }
            set { this.recordLevelCallHistory = value; }
        }

        [XmlAttribute("OnsiteTransfer")]
        public bool OnsiteTransfer
        {
            get { return this.onsiteTransfer; }
            set { this.onsiteTransfer = value; }
        }

        [XmlAttribute("EnableAgentTraining")]
        public bool EnableAgentTraining
        {
            get { return this.enableAgentTraining; }
            set { this.enableAgentTraining = value; }
        }

        [XmlAttribute("AllowDuplicatePhones")]
        public bool AllowDuplicatePhones
        {
            get { return this.allowDuplicatePhones; }
            set { this.allowDuplicatePhones = value; }
        }

        [XmlAttribute("Allow7DigitNums")]
        public bool Allow7DigitNums
        {
            get { return this.allow7DigitNums; }
            set { this.allow7DigitNums = value; }
        }

        [XmlAttribute("Allow10DigitNums")]
        public bool Allow10DigitNums
        {
            get { return this.allow10DigitNums; }
            set { this.allow10DigitNums = value; }
        }

        [XmlAttribute("DuplicateRule")]
        public string DuplicateRule
        {
            get { return this.duplicateRule; }
            set { this.duplicateRule = value; }
        }

        [XmlAttribute("FlushCallQueueOnIdle")]
        public bool FlushCallQueueOnIdle
        {
            get { return this.flushCallQueueOnIdle; }
            set { this.flushCallQueueOnIdle = value; }
        }

        [XmlAttribute("DialAllNumbers")]
        public bool DialAllNumbers
        {
            get { return this.dialAllNumbers; }
            set { this.dialAllNumbers = value; }
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

        [XmlAttribute("OutboundCallerID")]
        public string OutboundCallerID
        {
            get { return this.outboundCallerID; }
            set { this.outboundCallerID = value; }
        }

        [XmlAttribute("StartTime")]
        public DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        [XmlAttribute("StopTime")]
        public DateTime StopTime
        {
            get { return this.stopTime; }
            set { this.stopTime = value; }
        }
    }
}
