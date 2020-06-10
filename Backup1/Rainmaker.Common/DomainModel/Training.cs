using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{

    [Serializable()]
    [XmlRoot("TrainingPage")]
    public class TrainingPage
    {
        // A page for the training module.  Will add media support, etc for future phases as needed
        private long pageID;
        [XmlAttribute("PageID")]
        public long PageID
        {
            get { return this.pageID; }
            set { this.pageID = value; }
        }

        private long trainingSchemeID;
        [XmlAttribute("TrainingSchemeID")]
        public long TrainingSchemeID
        {
            get { return this.trainingSchemeID; }
            set { this.trainingSchemeID = value; }
        }

        private int displayTime;
        [XmlAttribute("DisplayTime")]
        public int DisplayTime
        {
            get { return this.displayTime; }
            set { this.displayTime = value; }
        }

        private string content;
        [XmlAttribute("HTMLString")]
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        private string name;
        [XmlAttribute("Name")]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        private bool isActive;
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
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

        private bool isScoreBoard;
        [XmlAttribute("IsScoreBoard")]
        public bool IsScoreBoard
        {
            get { return this.isScoreBoard; }
            set { this.isScoreBoard = value; }
        }
    }

    [Serializable()]
    [XmlRoot("TrainingScheme")]
    public class TrainingScheme
    {
        // A page for the training module.  Will add media support, etc for future phases as needed
        private long schemeID;
        [XmlAttribute("SchemeID")]
        public long SchemeID
        {
            get { return this.schemeID; }
            set { this.schemeID = value; }
        }

        private string name;
        [XmlAttribute("Name")]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        private int scoreboardFrequency = 5;
        [XmlAttribute("ScoreboardFrequency")]
        public int ScoreboardFrequency
        {
            get { return this.scoreboardFrequency; }
            set { this.scoreboardFrequency = value; }
        }

        private int scoreboardDisplayTime = 20;
        [XmlAttribute("ScoreboardDisplayTime")]
        public int ScoreboardDisplayTime
        {
            get { return this.scoreboardDisplayTime; }
            set { this.scoreboardDisplayTime = value; }
        }

        private int pageCount;
        [XmlAttribute("PageCount")]
        public int PageCount
        {
            get { return this.pageCount; }
            set { this.pageCount = value; }
        }

        private bool isActive;
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }
    }
}
