using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    [Serializable()]
    [XmlRoot("Script")]
    public class Script
    {
        public Script()
        {
            resetScript();
        }

        public void resetScript()
        {
            this.scriptID = 0;
            this.scriptName = "";
            this.scriptHeader = "";
            this.scriptSubHeader = "";
            this.scriptBody = "";
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
            this.parentScriptID = 0;
            this.scriptGuid = "";
            this.parentScriptName = "";
        }

        private long scriptID;
        [XmlAttribute("ScriptID")]
        public long ScriptID
        {
            get { return this.scriptID; }
            set { this.scriptID = value; }
        }

        private string scriptName;
        [XmlAttribute("ScriptName")]
        public string ScriptName
        {
            get { return this.scriptName; }
            set { this.scriptName = value; }
        }

        private string scriptHeader;
        [XmlAttribute("scriptHeader")]
        public string ScriptHeader
        {
            get { return this.scriptHeader; }
            set { this.scriptHeader = value; }
        }

        private string scriptSubHeader;
        [XmlAttribute("ScriptSubHeader")]
        public string ScriptSubHeader
        {
            get { return this.scriptSubHeader; }
            set { this.scriptSubHeader = value; }
        }

        private string scriptBody;
        [XmlAttribute("ScriptBody")]
        public string ScriptBody
        {
            get { return this.scriptBody; }
            set { this.scriptBody = value; }
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

        private long parentScriptID;
        [XmlAttribute("ParentScriptID")]
        public long ParentScriptID
        {
            get { return this.parentScriptID; }
            set { this.parentScriptID = value; }
        }

        private string scriptGuid;
        [XmlAttribute("ScriptGuid")]
        public string ScriptGuid
        {
            get { return this.scriptGuid; }
            set { this.scriptGuid = value; }
        }

        private string parentScriptName;
        [XmlAttribute("ParentScriptName")]
        public string ParentScriptName
        {
            get { return this.parentScriptName; }
            set { this.parentScriptName = value; }
        }
        
    }
}
