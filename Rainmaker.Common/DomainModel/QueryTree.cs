using System;
using System.Collections.Generic;
using System.Text;

namespace Rainmaker.Common.DomainModel
{
    public class QueryTree:Dictionary<int, QueryTreeItem>
    {

    }

    public class QueryTreeItem
    {
        private int treeNodeID = 0;
        private int parentTreeNodeID = 0;
        private long queryDetailID = 0;
        private long queryID = 0;
        private string queryName = "";
        private string searchCriteria = "";
        private string searchOperator = "";
        private string searchString = "";
        private bool isDateField = false;
       
        private int logicalOrder = 0;
        private long subQueryID = 0;
        private string subQueryName = "";
        private int subsetID = 0;
        private string subsetName = "";
        private int subsetLevel = 0;
        private int parentSubsetID = 0;
        private long parentQueryDetailID = 0;
        private int subsetLogicalOrder = 0;
        private DateTime dateCreated;
        private DateTime dateModified;
        private int newNodeAddIndex;

        // Label and SQL generation properties
        private bool applyLogicalOperator = false;
        private string logicalOperator = "";
        private int openParenPrefixCount = 0;
        private int closeParenSuffixCount = 0;

        // Node attributes
        private NodeType nodeType;
        private string nodeLabel = "";
        private string subQueryConditions = "";
        private string nodeLabelNameSubstitute = "";
        private string parentValuePath = "";

        public int TreeNodeID
        {
            get { return this.treeNodeID; }
            set { this.treeNodeID = value; }
        }

        public int ParentTreeNodeID
        {
            get { return this.parentTreeNodeID; }
            set { this.parentTreeNodeID = value; }
        }

        public int SubsetID
        {
            get { return this.subsetID; }
            set { this.subsetID = value; }
        }

        public long QueryDetailID
        {
            get { return this.queryDetailID; }
            set { this.queryDetailID = value; }
        }

        public long QueryID
        {
            get { return this.queryID; }
            set { this.queryID = value; }
        }

        public string QueryName
        {
            get { return this.queryName; }
            set { this.queryName = value; }
        }

        public string SearchCriteria
        {
            get { return this.searchCriteria; }
            set { this.searchCriteria = value; }
        }

        public string SearchOperator
        {
            get { return this.searchOperator; }
            set { this.searchOperator = value; }
        }

        public string SearchString
        {
            get { return this.searchString; }
            set { this.searchString = value; }
        }

        public bool IsDateField
        {
            get { return this.isDateField; }
            set { this.isDateField = value; }
        }

        public string LogicalOperator
        {
            get { return this.logicalOperator; }
            set { this.logicalOperator = value; }
        }

        public int LogicalOrder
        {
            get { return this.logicalOrder; }
            set { this.logicalOrder = value; }
        }

        public long SubQueryID
        {
            get { return this.subQueryID; }
            set { this.subQueryID = value; }
        }

        public string SubQueryName
        {
            get { return this.subQueryName; }
            set { this.subQueryName = value; }
        }

        public string SubsetName
        {
            get { return this.subsetName; }
            set { this.subsetName = value; }
        }

        public int SubsetLevel
        {
            get { return this.subsetLevel; }
            set { this.subsetLevel = value; }
        }

        public int ParentSubsetID
        {
            get { return this.parentSubsetID; }
            set { this.parentSubsetID = value; }
        }

        public long ParentQueryDetailID
        {
            get { return this.parentQueryDetailID; }
            set { this.parentQueryDetailID = value; }
        }

        public int SubsetLogicalOrder
        {
            get { return this.subsetLogicalOrder; }
            set { this.subsetLogicalOrder = value; }
        }

        public NodeType NodeType
        {
            get { return this.nodeType; }
            set { this.nodeType = value; }
        }

        public string NodeLabel
        {
            get { return this.nodeLabel; }
            set { this.nodeLabel = value; }
        }


        public bool ApplyLogicalOperator
        {
            get { return this.applyLogicalOperator; }
            set { this.applyLogicalOperator = value; }
        }

        public int OpenParenPrefixCount
        {
            get { return this.openParenPrefixCount; }
            set { this.openParenPrefixCount = value; }
        }

        public int CloseParenSuffixCount
        {
            get { return this.closeParenSuffixCount; }
            set { this.closeParenSuffixCount = value; }
        }

        public string SubQueryConditions
        {
            get { return this.subQueryConditions; }
            set { this.subQueryConditions = value; }
        }

        public DateTime DateCreated
        {
            get { return this.dateCreated; }
            set { this.dateCreated = value; }
        }

        public DateTime DateModified
        {
            get { return this.dateModified; }
            set { this.dateModified = value; }
        }

        public int NewNodeAddIndex
        {
            get { return this.newNodeAddIndex; }
            set { this.newNodeAddIndex = value; }
        }

        public string NodeLabelNameSubstitute
        {
            get { return this.nodeLabelNameSubstitute; }
            set { this.nodeLabelNameSubstitute = value; }
        }

        public string ParentValuePath
        {
            get { return this.parentValuePath; }
            set { this.parentValuePath = value; }
        }
    }

    public enum NodeType
        {
            RootCondition = 1,
            SubQuery = 2,
            SubSetParent = 3,
            SubSetChildCondition = 4,
            RootNode = 5
        }
}
