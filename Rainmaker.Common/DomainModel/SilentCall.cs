using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public class SilentCall
    {
        public SilentCall()
        {
        }

        private long silentCallID = 0;
        [XmlAttribute("SilentCallID")]
        public long SilentCallID
        {
            get { return this.silentCallID; }
            set { this.silentCallID = value; }
        }

        private long uniqueKey;
        [XmlAttribute("UniqueKey")]
        public long UniqueKey
        {
            get { return this.uniqueKey; }
            set { this.uniqueKey = value; }
        }

        private DateTime dateTimeofCall;
        [XmlAttribute("DateTimeofCall")]
        public DateTime DateTimeofCall
        {
            get { return this.dateTimeofCall; }
            set { this.dateTimeofCall = value; }
        }
    }
}
