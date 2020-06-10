using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("CloneInfo")]
    public class CloneInfo
    {
        private bool includeData;
        private bool includeFields;
        private bool includeQueries;
        private bool includeOptions; //Dial Params and scripts
        private bool includeResultCodes;
        private bool includeScripts;
        private bool fullCopy;
        private long parentCampaignId;
        private string parentShortDesc;
        private string recordingsPath;

        public CloneInfo()
        {
            ResetCloneInfo();
        }

        public void ResetCloneInfo()
        {
            includeData = false;
            includeFields = false;
            includeQueries = false;
            includeOptions = false; 
            includeResultCodes = false;
            includeScripts = false;
            parentCampaignId = 0;
            parentShortDesc = string.Empty;
            recordingsPath = string.Empty;
        }

        [XmlAttribute("IncludeData")]
        public bool IncludeData
        {
            get { return this.includeData; }
            set { this.includeData = value; }
        }

        [XmlAttribute("IncludeFields")]
        public bool IncludeFields
        {
            get { return this.includeFields; }
            set { this.includeFields = value; }
        }

        [XmlAttribute("IncludeQueries")]
        public bool IncludeQueries
        {
            get { return this.includeQueries; }
            set { this.includeQueries = value; }
        }

        [XmlAttribute("IncludeOptions")]
        public bool IncludeOptions
        {
            get { return this.includeOptions; }
            set { this.includeOptions = value; }
        }

        [XmlAttribute("IncludeResultCodes")]
        public bool IncludeResultCodes
        {
            get { return this.includeResultCodes; }
            set { this.includeResultCodes = value; }
        }

        [XmlAttribute("IncludeScripts")]
        public bool IncludeScripts
        {
            get { return this.includeScripts; }
            set { this.includeScripts = value; }
        }

        [XmlAttribute("FullCopy")]
        public bool FullCopy
        {
            get { return this.fullCopy; }
            set { this.fullCopy = value; }
        }

        [XmlAttribute("ParentCampaignId")]
        public long ParentCampaignId
        {
            get { return this.parentCampaignId; }
            set { this.parentCampaignId = value; }
        }

        [XmlAttribute("ParentShortDesc")]
        public string ParentShortDesc
        {
            get { return this.parentShortDesc; }
            set { this.parentShortDesc = value; }
        }
        [XmlAttribute("recordingsPath")]
        public string RecordingsPath
        {
            get { return this.recordingsPath; }
            set { this.recordingsPath = value; }
        }
    }
}
