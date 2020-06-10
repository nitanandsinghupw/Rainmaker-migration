using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("CampaignFields")]
    public class CampaignFields
    {
        public CampaignFields()
        {
            ResetCampaignFields();
        }

        public void ResetCampaignFields()
        {
            this.fieldTypeID = 0;
            this.fieldID = 0;
            this.dbValue = 0;
            this.fieldName = string.Empty;
            this.fieldType = string.Empty;
            this.isDefault = false;
            this.dbFieldType = string.Empty;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long fieldID;
        [XmlAttribute("FieldID")]
        public long FieldID
        {
            get { return this.fieldID; }
            set { this.fieldID = value; }
        }

        private string fieldName;
        [XmlAttribute("FieldName")]
        public string FieldName
        {
            get { return this.fieldName; }
            set { this.fieldName = value; }
        }

        private long fieldTypeID;
        [XmlAttribute("FieldTypeID")]
        public long FieldTypeID
        {
            get { return this.fieldTypeID; }
            set { this.fieldTypeID = value; }
        }

        private bool isDefault;
        [XmlAttribute("IsDefault")]
        public bool IsDefault
        {
            get { return this.isDefault; }
            set { this.isDefault = value; }
        }

        private int dbValue;
        [XmlAttribute("DBValue")]
        public int DBValue
        {
            get { return this.dbValue; }
            set { this.dbValue = value; }
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
