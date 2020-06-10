using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public class Station
    {
        public Station()
        {
           ResetStation();
        }

        private void ResetStation()
        {
            this.stationID = 0;
            this.stationIP = "";
            this.stationNumber = "";
            this.allwaysOffHook = false;
        }

        private long stationID;
        [XmlAttribute("StationID")]
        public long StationID
        {
            get { return this.stationID; }
            set { this.stationID = value; }
        }

        private string stationIP;
        [XmlAttribute("StationIP")]
        public string StationIP
        {
            get { return this.stationIP; }
            set { this.stationIP = value; }
        }

        private string stationNumber;
        [XmlAttribute("StationNumber")]
        public string StationNumber
        {
            get { return this.stationNumber; }
            set { this.stationNumber = value; }
        }

        private bool allwaysOffHook;
        [XmlAttribute("AllwaysOffHook")]
        public bool AllwaysOffHook
        {
            get { return this.allwaysOffHook; }
            set { this.allwaysOffHook = value; }
        }
    }
}
