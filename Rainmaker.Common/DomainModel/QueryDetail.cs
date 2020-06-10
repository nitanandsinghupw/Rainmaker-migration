using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    
    [Serializable()]
    [XmlRoot("QueryDetail")]
    public class QueryDetail
    {
        public QueryDetail()
        {
            ResetQueryDetail();
        }

        public void ResetQueryDetail()
        {
            this.queryDetailID = 0;
            this.queryID = 0;
            this.searchCriteria = "";
            this.searchOperator = "";
            this.searchString = "";
            this.logicalOperator = "";
            this.logicalOrder = 0;
            this.subQueryID = 0;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.subsetID = 0;
            this.subsetName = "";
            this.subsetLevel = 0;
            this.parentSubsetID = 0;
            this.parentQueryDetailID = 0;
            this.subsetLogicalOrder = 0;
            this.elementText = "";
        }

        private long queryDetailID;
        [XmlAttribute("QueryDetailID")]
        public long QueryDetailID
        {
            get { return this.queryDetailID; }
            set { this.queryDetailID = value; }
        }

        private long queryID;
        [XmlAttribute("QueryID")]
        public long QueryID
        {
            get { return this.queryID; }
            set { this.queryID = value; }
        }

        private string searchCriteria;
        [XmlAttribute("SearchCriteria")]
        public string SearchCriteria
        {
            get { return this.searchCriteria; }
            set { this.searchCriteria = value; }
        }

        private string searchOperator;
        [XmlAttribute("SearchOperator")]
        public string SearchOperator
        {
            get { return this.searchOperator; }
            set { this.searchOperator = value; }
        }

        private string searchString;
        [XmlAttribute("SearchString")]
        public string SearchString
        {
            get { return this.searchString; }
            set { this.searchString = value; }
        }

        private string logicalOperator;
        [XmlAttribute("LogicalOperator")]
        public string LogicalOperator
        {
            get { return this.logicalOperator; }
            set { this.logicalOperator = value; }
        }

        private int logicalOrder;
        [XmlAttribute("LogicalOrder")]
        public int LogicalOrder
        {
            get { return this.logicalOrder; }
            set { this.logicalOrder = value; }
        }

        private long subQueryID;
        [XmlAttribute("SubQueryID")]
        public long SubQueryID
        {
            get { return this.subQueryID; }
            set { this.subQueryID = value; }
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
       
        private int subsetID = 0;
        [XmlAttribute("SubsetID")]
        public int SubsetID
        {
            get { return this.subsetID; }
            set { this.subsetID = value; }
        }

        private string subsetName = "";
        [XmlAttribute("SubsetName")]
        public string SubsetName
        {
            get { return this.subsetName; }
            set { this.subsetName = value; }
        }

        private int subsetLevel = 0;
        [XmlAttribute("SubsetLevel")]
        public int SubsetLevel
        {
            get { return this.subsetLevel; }
            set { this.subsetLevel = value; }
        }

        private int parentSubsetID = 0;
        [XmlAttribute("ParentSubsetID")]
        public int ParentSubsetID
        {
            get { return this.parentSubsetID; }
            set { this.parentSubsetID = value; }
        }

        private long parentQueryDetailID = 0;
        [XmlAttribute("ParentQueryDetailID")]
        public long ParentQueryDetailID
        {
            get { return this.parentQueryDetailID; }
            set { this.parentQueryDetailID = value; }
        }

        private int subsetLogicalOrder = 0;
        [XmlAttribute("SubsetLogicalOrder")]
        public int SubsetLogicalOrder
        {
            get { return this.subsetLogicalOrder; }
            set { this.subsetLogicalOrder = value; }
        }

        private string elementText = "";
        [XmlAttribute("ElementText")]
        public string ElementText
        {
            get { return this.elementText; }
            set { this.elementText = value; }
        }

        private int treeNodeID = 0;
        [XmlAttribute("TreeNodeID")]
        public int TreeNodeID
        {
            get { return this.treeNodeID; }
            set { this.treeNodeID = value; }
        }

        private int parentTreeNodeID = 0;
        [XmlAttribute("ParentTreeNodeID")]
        public int ParentTreeNodeID
        {
            get { return this.parentTreeNodeID; }
            set { this.parentTreeNodeID = value; }
        }
    }
}
