using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("ResultCode")]
    public class ResultCode
    {
        public ResultCode()
        {
            //ResetResultCode();
        }

        public void ResetResultCode()
        {
            this.resultCodeID = 0;
            this.description = "";
            this.presentation = false;
            this.redialable = true;
            this.recycleInDays = 0;
            this.lead = false;
            this.masterDNC = false;
            this.neverCall = false;
            this.verifyOnly = false;
            this.liveContact = false;
            this.dialThroughAll = false;
            this.showDeletedResultCodes = false;
            this.dateDeleted = DateTime.Now;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long resultCodeID;
        [XmlAttribute("ResultCodeID")]
        public long ResultCodeID
        {
            get { return this.resultCodeID; }
            set { this.resultCodeID = value; }
        }

        private string description;
        [XmlAttribute("Description")]
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        private bool presentation;
        [XmlAttribute("Presentation")]
        public bool Presentation
        {
            get { return this.presentation; }
            set { this.presentation = value; }
        }

        private bool redialable;
        [XmlAttribute("Redialable")]
        public bool Redialable
        {
            get { return this.redialable; }
            set { this.redialable = value; }
        }

        private int recycleInDays;
        [XmlAttribute("RecycleInDays")]
        public int RecycleInDays
        {
            get { return this.recycleInDays; }
            set { this.recycleInDays = value; }
        }

        private bool lead;
        [XmlAttribute("Lead")]
        public bool Lead
        {
            get { return this.lead; }
            set { this.lead = value; }
        }

        private bool masterDNC;
        [XmlAttribute("MasterDNC")]
        public bool MasterDNC
        {
            get { return this.masterDNC; }
            set { this.masterDNC = value; }
        }

        private bool neverCall;
        [XmlAttribute("NeverCall")]
        public bool NeverCall
        {
            get { return this.neverCall; }
            set { this.neverCall = value; }
        }

        private bool verifyOnly;
        [XmlAttribute("VerifyOnly")]
        public bool VerifyOnly
        {
            get { return this.verifyOnly; }
            set { this.verifyOnly = value; }
        }

        private bool liveContact;
        [XmlAttribute("LiveContact")]
        public bool LiveContact
        {
            get { return this.liveContact; }
            set { this.liveContact = value; }
        }

        private bool dialThroughAll;
        [XmlAttribute("DialThroughAll")]
        public bool DialThroughAll
        {
            get { return this.dialThroughAll; }
            set { this.dialThroughAll = value; }
        }

        private bool showDeletedResultCodes;
        [XmlAttribute("ShowDeletedResultCodes")]
        public bool ShowDeletedResultCodes
        {
            get { return this.showDeletedResultCodes; }
            set { this.showDeletedResultCodes = value; }
        }

        private DateTime dateDeleted;
        [XmlAttribute("DateDeleted")]
        public DateTime DateDeleted
        {
            get { return this.dateDeleted; }
            set { this.dateDeleted = value; }
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
