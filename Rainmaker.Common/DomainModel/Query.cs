using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("Query")]
    public class Query
    {
        public Query()
        {
            ResetQuery();
        }

        public void ResetQuery()
        {
            this.queryID = 0;
            this.queryName = "";
            this.queryCondition = "";
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
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

        private string queryCondition;
        [XmlAttribute("QueryCondition")]
        public string QueryCondition
        {
            get { return this.queryCondition; }
            set { this.queryCondition = value; }
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
