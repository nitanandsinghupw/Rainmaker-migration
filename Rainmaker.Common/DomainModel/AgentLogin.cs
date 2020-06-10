using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{

    public enum AgentLoginStatus
    {
        Ready = 1,
        Pause = 2,
        Busy = 3
    }

    public enum AgentCallReceiptMode
    {
        OutboundOnly = 1,
        VerifyOnly = 2,
        VerifyBlended = 3,
        InboundOnly = 4,
        ManualDial = 5
    }

    public class AgentLogin
    {
        public AgentLogin()
        {
            ResetAgentLogin();
        }

        public void ResetAgentLogin()
        {
            this.agentLoginID = 0;
            this.agentID = 0;
            this.loginStatusID = 0;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long agentLoginID;
        [XmlAttribute("AgentLoginID")]
        public long AgentLoginID
        {
            get { return this.agentLoginID; }
            set { this.agentLoginID = value; }
        }

        private long agentID;
        [XmlAttribute("AgentID")]
        public long AgentID
        {
            get { return this.agentID; }
            set { this.agentID = value; }
        }

        private long loginStatusID;
        [XmlAttribute("LoginStatusID")]
        public long LoginStatusID
        {
            get { return this.loginStatusID; }
            set { this.loginStatusID = value; }
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
