using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("AreaCodeRule")]
    public class AreaCodeRule
    {
        public AreaCodeRule()
        {
            ResetAreaCodeRule();
        }

        public void ResetAreaCodeRule()
        {
            this.areaCodeRuleID = 0;
            this.agentID = 0;
            this.areaCodeID = 0;
            this.likeDialing = true;
            this.likeDialingOption = false;
            this.customeDialing = false;
            this.isSevenDigit = false;
            this.isTenDigit = false;
            this.intraLataDialing = false;
            this.intraLataDialingAreaCode = "";
            this.ildIsTenDigit = false;
            this.ildElevenDigit = false;
            this.replaceAreaCode = "";
            this.longDistanceDialing = false;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long areaCodeRuleID;
        [XmlAttribute("AreaCodeRuleID")]
        public long AreaCodeRuleID
        {
            get { return this.areaCodeRuleID; }
            set { this.areaCodeRuleID = value; }
        }

        private long agentID;
        [XmlAttribute("AgentID")]
        public long AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
        }

        private long areaCodeID;
        [XmlAttribute("AreaCodeID")]
        public long AreaCodeID
        {
            get { return this.areaCodeID; }
            set { this.areaCodeID = value; }
        }

        private bool likeDialing;
        [XmlAttribute("LikeDialing")]
        public bool LikeDialing
        {
            get { return this.likeDialing; }
            set { this.likeDialing = value; }
        }

        private bool likeDialingOption;
        [XmlAttribute("LikeDialingOption")]
        public bool LikeDialingOption
        {
            get { return this.likeDialingOption; }
            set { this.likeDialingOption = value; }
        }

        private bool customeDialing;
        [XmlAttribute("CustomeDialing")]
        public bool CustomeDialing
        {
            get { return this.customeDialing; }
            set { this.customeDialing = value; }
        }

        private bool isSevenDigit;
        [XmlAttribute("IsSevenDigit")]
        public bool IsSevenDigit
        {
            get { return this.isSevenDigit; }
            set { this.isSevenDigit = value; }
        }

        private bool isTenDigit;
        [XmlAttribute("IsTenDigit")]
        public bool IsTenDigit
        {
            get { return this.isTenDigit; }
            set { this.isTenDigit = value; }
        }

        private bool intraLataDialing;
        [XmlAttribute("IntraLataDialing")]
        public bool IntraLataDialing
        {
            get { return this.intraLataDialing; }
            set { this.intraLataDialing = value; }
        }

        private string intraLataDialingAreaCode;
        [XmlAttribute("IntraLataDialingAreaCode")]
        public string IntraLataDialingAreaCode
        {
            get { return this.intraLataDialingAreaCode; }
            set { this.intraLataDialingAreaCode = value; }
        }

        private bool ildIsTenDigit;
        [XmlAttribute("ILDIsTenDigit")]
        public bool ILDIsTenDigit
        {
            get { return this.ildIsTenDigit; }
            set { this.ildIsTenDigit = value; }
        }

        private bool ildElevenDigit;
        [XmlAttribute("ILDElevenDigit")]
        public bool ILDElevenDigit
        {
            get { return this.ildElevenDigit; }
            set { this.ildElevenDigit = value; }
        }

        private string replaceAreaCode;
        [XmlAttribute("ReplaceAreaCode")]
        public string ReplaceAreaCode
        {
            get { return this.replaceAreaCode; }
            set { this.replaceAreaCode = value; }
        }

        private bool longDistanceDialing;
        [XmlAttribute("LongDistanceDialing")]
        public bool LongDistanceDialing
        {
            get { return this.longDistanceDialing; }
            set { this.longDistanceDialing = value; }
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
