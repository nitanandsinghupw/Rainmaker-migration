using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("FieldTypes")]
    public class FieldTypes
    {
        public FieldTypes()
        {
            ResetFieldTypes();
        }

        public void ResetFieldTypes()
        {
            this.fieldTypeID = 0;
            this.fieldType = string.Empty;
            this.dbFieldType = string.Empty;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }


        private long fieldTypeID;
        [XmlAttribute("FieldTypeID")]
        public long FieldTypeID
        {
            get { return this.fieldTypeID; }
            set { this.fieldTypeID = value; }
        }

        private string fieldType;
        [XmlAttribute("FieldType")]
        public string FieldType
        {
            get { return this.fieldType; }
            set { this.fieldType = value; }
        }

        private string dbFieldType;
        [XmlAttribute("DbFieldType")]
        public string DbFieldType
        {
            get { return this.dbFieldType; }
            set { this.dbFieldType = value; }
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
