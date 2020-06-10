using System;
using System.Collections.Generic;
using System.Text;
using Rainmaker.Common.DomainModel;
using Rainmaker.RainmakerDialer;
using System.Threading;
using System.Data;
using System.Data.DataSetExtensions;
using System.Linq;

namespace RainMakerDialer.Dialer
{
    public delegate void CampaignMonitorEventHandler(Object sender, EventArgs e);

    public class CampaignMonitor
    {
        public event CampaignMonitorEventHandler campaignStateChangeEvent;
        public event CampaignMonitorEventHandler queryStateChangeEvent;

        private Campaign campaign;
        private Dictionary<long, Query> activeQueries;

        public CampaignMonitor(Campaign campaign)
        {
            this.campaign = campaign;
            activeQueries = new Dictionary<long, Query>();
        }

        public void monitor(Object campaignProcessThread)
        {
            while ((campaignProcessThread as Thread).IsAlive)
            {
                CampaignStatus campaignStatus = (CampaignStatus)CampaignAPI.GetCampaignStatus(campaign.CampaignID);
                if (campaign.StatusID != (long)campaignStatus)
                {
                    campaign.StatusID = (long)campaignStatus;
                    campaignStateChangeEvent(this, new CampaignStateChangeEventArgs(campaignStatus));
                }

                IEnumerable<DataRow> activeQueryRows = CampaignAPI.GetActiveQueries(campaign).Tables[0].AsEnumerable();

                IEnumerable<Query> newActiveQueries =
                    from newActiveQuery in activeQueryRows
                    where !activeQueries.ContainsKey((long)newActiveQuery["QueryID"])
                    select new Query()
                    {
                        QueryID = (long)newActiveQuery["QueryID"],
                        QueryName = (string)newActiveQuery["QueryName"],
                        QueryCondition = (string)newActiveQuery["QueryCondition"]
                    };

                List<long> inactiveQueryKeys = activeQueries.Keys.Except(activeQueryRows.Select(x => (long)x["QueryID"])).ToList<long>();

                foreach (Query query in newActiveQueries)
                {
                    activeQueries.Add(query.QueryID, query);
                    queryStateChangeEvent(this, new QueryStateChangeEventArgs(query, QueryState.active));
                }

                foreach (long queryKey in inactiveQueryKeys)
                {
                    queryStateChangeEvent(this, new QueryStateChangeEventArgs(activeQueries[queryKey], QueryState.inactive));
                    activeQueries.Remove(queryKey);
                }

                Thread.Sleep(5000);
            }

            DialerEngine.Log.Write("|CM|{0}|{1}|EXITING CAMPAIGN MONITOR.", campaign.CampaignID, campaign.ShortDescription);
        }
    }

    public enum QueryState
    {
        active,
        inactive
    }

    public class QueryStateChangeEventArgs : EventArgs
    {
        public QueryStateChangeEventArgs(Query query, QueryState state)
        {
            this.query = query;
            this.state = state;
        }

        public QueryState state { get; set; }
        public Query query { get; set; }
    }

    public class CampaignStateChangeEventArgs : EventArgs
    {
        public CampaignStateChangeEventArgs(CampaignStatus campaignStatus)
        {
            this.status = campaignStatus;
        }

        public CampaignStatus status { get; set; }
    }
}
