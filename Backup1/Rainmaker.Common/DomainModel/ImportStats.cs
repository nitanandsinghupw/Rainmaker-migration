using System;
using System.Collections.Generic;
using System.Text;

namespace Rainmaker.Common.DomainModel
{
    public class ImportStats
    {
        private int leadsImported = 0;
        public int LeadsImported
        {
            get
            {
                return leadsImported;
            }
            set
            {
                this.leadsImported = value;
            }
        }

        private int totalLeads = 0;
        public int TotalLeads
        {
            get
            {
                return totalLeads;
            }
            set
            {
                this.totalLeads = value;
            }
        }

        private int leadsBlankPhoneNumber = 0;
        public int LeadsBlankPhoneNumber
        {
            get
            {
                return leadsBlankPhoneNumber;
            }
            set
            {
                this.leadsBlankPhoneNumber = value;
            }
        }

        private int leadsSPCharPhoneNumber = 0;
        public int LeadsSPCharPhoneNumber
        {
            get
            {
                return leadsSPCharPhoneNumber;
            }
            set
            {
                this.leadsSPCharPhoneNumber = value;
            }
        }

        private int leadsBadData = 0;
        public int LeadsBadData
        {
            get
            {
                return leadsBadData;
            }
            set
            {
                this.leadsBadData = value;
            }
        }

        private int leadsInvalidNumberLength = 0;
        public int LeadsInvalidNumberLength
        {
            get
            {
                return leadsInvalidNumberLength;
            }
            set
            {
                this.leadsInvalidNumberLength = value;
            }
        }

        private int leadsDuplicate = 0;
        public int LeadsDuplicate
        {
            get
            {
                return leadsDuplicate;
            }
            set
            {
                this.leadsDuplicate = value;
            }
        }

        private int leadsUpdated = 0;
        public int LeadsUpdated
        {
            get
            {
                return leadsUpdated;
            }
            set
            {
                this.leadsUpdated = value;
            }
        }
    }
}
