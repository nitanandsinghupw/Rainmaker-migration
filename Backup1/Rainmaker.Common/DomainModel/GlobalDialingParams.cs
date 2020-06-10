using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public class GlobalDialingParams
    {
        public GlobalDialingParams()
        {
            ResetGlobalDialingParams();
        }

        public void ResetGlobalDialingParams()
        {
            this.globalDialingID = 0;
            this.prefix = "";
            this.suffix = "";
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long globalDialingID;
        [XmlAttribute("GlobalDialingID")]
        public long GlobalDialingID
        {
            get { return this.globalDialingID; }
            set { this.globalDialingID = value; }
        }

        private string prefix;
        [XmlAttribute("Prefix")]
        public string Prefix
        {
            get { return this.prefix; }
            set { this.prefix = value; }
        }

        private string suffix;
        [XmlAttribute("Suffix")]
        public string Suffix
        {
            get { return this.suffix; }
            set { this.suffix = value; }
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
