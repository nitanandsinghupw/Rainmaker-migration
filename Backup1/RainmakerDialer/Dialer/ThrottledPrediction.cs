using System;
using System.Collections.Generic;
using System.Text;
using VoiceElements.Common;
using Rainmaker.Common.DomainModel;
using System.Diagnostics;

namespace Rainmaker.RainmakerDialer
{
    class ThrottledPrediction
    {
         #region Variables and Properties
        
            private Campaign objCampaign = null;

            private decimal m_AvgAgentBusyTime = 0.0m;
            private decimal m_AvgTimeToAnswer = 0.0m;
            private decimal m_ProbabilityOfAnswer = 0.0m;
            private decimal m_AvgCallTime = 0.0m;
            private decimal _lastDropRate = 0.0m;
            private int m_RequiredDialingCalls = 0;

            private Stopwatch _dropRateTimer = new Stopwatch();

         #endregion

        #region Constructor
        public ThrottledPrediction(Campaign objCampaign)
        {
            this.objCampaign = objCampaign;
        }

        #endregion

        public int[] CalculateNextCallTime(DialingParameter dialingParameters, CampaignStats campStats, int totalAgentCount, int availableAgentCount, decimal currentDropRate, int currentlyDialingCallCount)
        {
            int[] delayOrCallCount = new int[2];
            // First calculate current Probability of Answer
            m_ProbabilityOfAnswer = GetProbabilityOfAnswer(campStats);
                
            // Check number of required dialing calls per throttle setting
            m_RequiredDialingCalls = (int)((1 - m_ProbabilityOfAnswer) * availableAgentCount) + availableAgentCount;

            if (currentlyDialingCallCount < m_RequiredDialingCalls)
            {
                delayOrCallCount[0] = (dialingParameters.MinimumDelayBetweenCalls * 1000);
                delayOrCallCount[1] = m_RequiredDialingCalls - currentlyDialingCallCount;
                // By throttle, we don't even have enough pending calls, fire immediate calls and exit.
                DialerEngine.Log.Write("|PR|{0}|{1}|Current POA vs agent inverse ratio of {2} requires {3} pending calls with min delay set to {4}, triggering {5} immediate calls.", objCampaign.CampaignID, objCampaign.ShortDescription, (1 - m_ProbabilityOfAnswer), m_RequiredDialingCalls, dialingParameters.MinimumDelayBetweenCalls, delayOrCallCount[1]);
                return delayOrCallCount;
            }

            // Check current drop rate and pause accordingly
            if (currentDropRate > dialingParameters.DropRatePercent)
            {
                // Drop rate threshold exceeded, pause dialing until it falls.  Keep in mind, the throttle, etc will still function because they are Before this trap
                DialerEngine.Log.Write("|PR|{0}|{1}|Drop rate of {2}% exceeds max setting of {3}%, pausing dialing.", objCampaign.CampaignID, objCampaign.ShortDescription, currentDropRate, dialingParameters.DropRatePercent);
                delayOrCallCount[0] = -1;
                return delayOrCallCount;
            }

            // Check acceleration towards drop rate
            decimal currentDropRateAcceleration = _dropRateTimer.IsRunning ? (_lastDropRate - currentDropRate) / (_dropRateTimer.ElapsedMilliseconds) * 60000 : 0.00m;
            _dropRateTimer.Reset();
            _dropRateTimer.Start();

            _lastDropRate = currentDropRate;

            decimal targetDropRateAcceleration = 
                dialingParameters.DropRatePercent > 0
                ? (1 - currentDropRate / dialingParameters.DropRatePercent) * dialingParameters.DropRateThrottle 
                : 0;

            if (currentDropRateAcceleration <= targetDropRateAcceleration)
            {
                DialerEngine.Log.Write
                (
                    "|PR|{0}|{1}|Drop rate of {2}% is under max setting of {3}%.  Drop Rate Acceleration Per Minute is Maximum: {4}, Current: {5}, Target: {6}. Keep dialing.",
                    objCampaign.CampaignID,
                    objCampaign.ShortDescription,
                    currentDropRate,
                    dialingParameters.DropRatePercent,
                    dialingParameters.DropRateThrottle,
                    currentDropRateAcceleration,
                    targetDropRateAcceleration
                );

                delayOrCallCount[0] = (dialingParameters.MinimumDelayBetweenCalls * 1000);

                return delayOrCallCount;
            }

            // Main Algorithm calculation, all above traps have been avoided, now we do a predictive calculation of next call time and return it.

            m_AvgAgentBusyTime = campStats.GetAAIUT();
            m_AvgTimeToAnswer = campStats.GetATTA();
            m_AvgCallTime = campStats.GetACT();

            decimal delayToNextCall = 0;

            if (m_AvgAgentBusyTime > m_AvgTimeToAnswer)
                    delayToNextCall = m_AvgAgentBusyTime - m_AvgTimeToAnswer + dialingParameters.MinimumDelayBetweenCalls;
            else
            {
                delayToNextCall = dialingParameters.MinimumDelayBetweenCalls;
                try
                {
                    delayToNextCall = Math.Max(delayToNextCall, m_AvgCallTime);
                }
                catch { }
            }

                
            if (totalAgentCount> 1)
                delayToNextCall /= totalAgentCount;

            delayOrCallCount[0] = Convert.ToInt32(Math.Floor(delayToNextCall * 1000));

            DialerEngine.Log.Write("|PR|{0}|{1}|Throttled algorithm calculated a delay of {5} MS: AABT - {2}, ATTA - {3}, ACT - {4}", objCampaign.CampaignID, objCampaign.ShortDescription, string.Format("{0:0.00}", m_AvgAgentBusyTime), string.Format("{0:0.00}", m_AvgTimeToAnswer), string.Format("{0:0.00}", m_AvgCallTime), delayOrCallCount[0]);

            return delayOrCallCount;
        }

        private decimal GetProbabilityOfAnswer(CampaignStats campStats)
        {
            decimal currentPOA = 0;
            int iAnswers = 0;
            try
            {
                lock (campStats.CallAnswerList)
                {
                    
                    for (int i = 0; i < campStats.CallAnswerList.Count; i++)
                    {
                        if (campStats.CallAnswerList[i])
                            iAnswers += 1;
                    }
                    if (campStats.CallAnswerList.Count > 0)
                    {
                        if (iAnswers > 0)
                            currentPOA = (decimal)iAnswers / (decimal)campStats.CallAnswerList.Count;
                        else
                            currentPOA = 0;
                    }
                    else
                    {
                        currentPOA = 0;
                    }
                }
                DialerEngine.Log.Write("|PR|{0}|{1}|{2} calls with {3} answers makes current probability of answer: {4}.", objCampaign.CampaignID, objCampaign.ShortDescription, campStats.CallAnswerList.Count, iAnswers, string.Format("{0:0.00}", currentPOA));
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Exception calculating POA.");
            }
            return currentPOA;
        }

    }
}
