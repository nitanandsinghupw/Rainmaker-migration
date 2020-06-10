using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public enum CallBackOptions
    {
        DonotAllowCallBacks = 1,
        AllowAgentCallBacks = 2,
        AllowStationCallBacks = 3,
        AllowSystemCallBacks=4,
        DonotAllowCallTransfer=1,
        AllowOffsiteCallTransfer=2,
        AllowOnSiteCallTransfer=3,
    }
    [Serializable()]
    [XmlRoot("OtherParameter")]
    public class OtherParameter
    {
        public OtherParameter()
        {
            ResetOtherParameter();
        }

        public void ResetOtherParameter()
        {
            this.otherParameterID = 0;
            this.callTransfer = 0;
            this.staticOffsiteNumber = "";
            this.allowOnSiteTranferWData = false;
            this.transferMessageEnable = false;
            this.transferMessage = "";
            this.holdMessage = "";
            this.allowManualDial = false;
            this.startingLine = 0;
            this.endingLine = -2;
            this.allowCallBacks = 0;
            this.alertSupervisorOnCallbacks = 0;
            this.queryStatisticsInPercent = true;
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long otherParameterID;
        [XmlAttribute("OtherParameterID")]
        public long OtherParameterID
        {
            get { return this.otherParameterID; }
            set { this.otherParameterID = value; }
        }

        private int callTransfer;
        [XmlAttribute("CallTransfer")]
        public int CallTransfer
        {
            get { return this.callTransfer; }
            set { this.callTransfer = value; }
        }

        private string staticOffsiteNumber;
        [XmlAttribute("StaticOffsiteNumber")]
        public string StaticOffsiteNumber
        {
            get { return this.staticOffsiteNumber; }
            set { this.staticOffsiteNumber = value; }
        }

        private bool allowOnSiteTranferWData;
        [XmlAttribute("AllowOnSiteTranferWData")]
        public bool AllowOnSiteTranferWData
        {
            get { return this.allowOnSiteTranferWData; }
            set { this.allowOnSiteTranferWData = value; }
        }

        private bool transferMessageEnable;
        [XmlAttribute("TransferMessageEnable")]
        public bool TransferMessageEnable
        {
            get { return this.transferMessageEnable; }
            set { this.transferMessageEnable = value; }
        }

        private string transferMessage;
        [XmlAttribute("AutoPlayMessage")]
        public string TransferMessage
        {
            get { return this.transferMessage; }
            set { this.transferMessage = value; }
        }

        private string holdMessage;
        [XmlAttribute("HoldMessage")]
        public string HoldMessage
        {
            get { return this.holdMessage; }
            set { this.holdMessage = value; }
        }

        private bool allowManualDial;
        [XmlAttribute("AllowManualDial")]
        public bool AllowManualDial
        {
            get { return this.allowManualDial; }
            set { this.allowManualDial = value; }
        }

        private int startingLine;
        [XmlAttribute("StartingLine")]
        public int StartingLine
        {
            get { return this.startingLine; }
            set { this.startingLine = value; }
        }
        private int endingLine;
        [XmlAttribute("EndingLine")]
        public int EndingLine
        {
            get { return this.endingLine; }
            set { this.endingLine = value; }
        }


        private int alertSupervisorOnCallbacks;
        [XmlAttribute("AlertSupervisorOnCallbacks")]
        public int AlertSupervisorOnCallbacks
        {
            get { return this.alertSupervisorOnCallbacks; }
            set { this.alertSupervisorOnCallbacks = value; }
        }

        private int allowCallBacks;
        [XmlAttribute("AllowCallBacks")]
        public int AllowCallBacks
        {
            get { return this.allowCallBacks; }
            set { this.allowCallBacks = value; }
        }

        private bool queryStatisticsInPercent;
        [XmlAttribute("QueryStatisticsInPercent")]
        public bool QueryStatisticsInPercent
        {
            get { return this.queryStatisticsInPercent; }
            set { this.queryStatisticsInPercent = value; }
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
