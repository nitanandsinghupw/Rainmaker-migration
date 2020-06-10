using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public class AreaCode
    {
        public AreaCode()
        {
            ResetAreaCode();
        }

        public void ResetAreaCode()
        {
            this.areaCodeID = 0;
            this.areaCodeName = "";
            this.prefix = "";
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long areaCodeID;
        [XmlAttribute("AreaCodeID")]
        public long AreaCodeID
        {
            get { return this.areaCodeID; }
            set { this.areaCodeID = value; }
        }

        private string areaCodeName;
        [XmlAttribute("AreaCodeName")]
        public string AreaCodeName
        {
            get { return this.areaCodeName; }
            set { this.areaCodeName = value; }
        }

        private string prefix;
        [XmlAttribute("Prefix")]
        public string Prefix
        {
            get { return this.prefix; }
            set { this.prefix = value; }
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
