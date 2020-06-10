using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("ImportFieldRow")]
    public class ImportFieldRow
    {
        public ImportFieldRow()
        {
        }

        private List<ImportField> importFieldsList = new List<ImportField>();
        [XmlArray()]
        public List<ImportField> ImportFieldsList
        {
            get { return this.importFieldsList; }
            set { this.importFieldsList = value; }
        }
    }

    [Serializable()]
    public class ImportField
    {
        public ImportField()
        {
        }

        private string fieldName;
        [XmlAttribute("FieldName")]
        public string FieldName
        {
            get { return this.fieldName; }
            set { this.fieldName = value; }
        }

        private string fieldType;
        [XmlAttribute("FieldType")]
        public string FieldType
        {
            get { return this.fieldType; }
            set { this.fieldType = value; }
        }

        private string fieldValue;
        [XmlAttribute("FieldValue")]
        public string FieldValue
        {
            get { return this.fieldValue; }
            set { this.fieldValue = value; }
        }
    }
}