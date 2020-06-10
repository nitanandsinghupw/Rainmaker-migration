using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{

    [Serializable()]
    [XmlRoot("DigitalizedRecording")]
    public class DigitalizedRecording
    {
        public DigitalizedRecording()
        {
            ResetDigitalizedRecording();
        }

        public void ResetDigitalizedRecording()
        {
            this.digitalizedRecordingID = 0;
            this.enableRecording = true;
            this.enableWithABeep = false;
            this.startWithABeep = true;
            this.recordToWave = true;
            this.highQualityRecording = false;
            this.recordingFilePath = "";
            this.fileNaming = "";
            this.dateCreated = DateTime.Now;
            this.dateModified = DateTime.Now;
        }

        private long digitalizedRecordingID;        
        [XmlAttribute("DigitalizedRecordingID")]
        public long DigitalizedRecordingID
        {
            get { return this.digitalizedRecordingID; }
            set { this.digitalizedRecordingID = value; }
        }

		private bool enableRecording;
        [XmlAttribute("EnableRecording")]
        public bool EnableRecording
        {
            get { return this.enableRecording; }
            set { this.enableRecording = value; }
        }

        private bool enableWithABeep;
        [XmlAttribute("EnableWithABeep")]
        public bool EnableWithABeep
        {
            get { return this.enableWithABeep; }
            set { this.enableWithABeep = value; }
        }

        private bool startWithABeep;
        [XmlAttribute("StartWithABeep")]
        public bool StartWithABeep
        {
            get { return this.startWithABeep; }
            set { this.startWithABeep = value; }
        }

        private bool recordToWave;
        [XmlAttribute("RecordToWave")]
        public bool RecordToWave
        {
            get { return this.recordToWave; }
            set { this.recordToWave = value; }
        }

        private bool highQualityRecording;
        [XmlAttribute("HighQualityRecording")]
        public bool HighQualityRecording
        {
            get { return this.highQualityRecording; }
            set { this.highQualityRecording = value; }
        }

        private string recordingFilePath;
        [XmlAttribute("RecordingFilePath")]
        public string RecordingFilePath
        {
            get { return this.recordingFilePath; }
            set { this.recordingFilePath = value; }
        }

        private string fileNaming;
        [XmlAttribute("FileNaming")]
        public string FileNaming
        {
            get { return this.fileNaming; }
            set { this.fileNaming = value; }
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
