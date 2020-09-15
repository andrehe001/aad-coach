using System;
using AdventureDayRunner.Model;
using team_management_api.Data;

namespace AdventureDayRunner.Players
{
    public class MatchReport
    {
        private const int FixedMatchStake = 10;

        private MatchReport()
        {
        }

        #region Creation Helpers
        public static MatchReport FromCostCalculator(int cost)
        {
            return new MatchReport()
            {
                MatchRatingStatus = MatchRating.Ignore,
                Income = 0,
                Cost = cost
            };
        }

        public static MatchReport FromMatchResponse(MatchResponse matchResponse)
        {
            var outcome = CalculateHumanTeamMoneyOutcome(matchResponse);

            var reason = outcome > 0
                ? $"Human has won ${outcome}"
                : $"Smoorgh has won ${outcome}";

            return new MatchReport()
            {
                MatchRatingStatus = MatchRating.PlayedMatchSuccessfully,
                LogEntryStatus = LogEntryStatus.SUCCESS,
                Reason = reason,
                Income = outcome > 0 ? outcome : 0,
                Cost =   outcome > 0 ? 0 : outcome
            };
        }

        public static MatchReport FromError(string error)
        {
            return new MatchReport()
            {
                MatchRatingStatus = MatchRating.Error,
                LogEntryStatus = LogEntryStatus.FAILED,
                Reason = error,
                Income = 0,
                Cost = 0
            };
        }
        
        public static MatchReport FromCancellation(string reason)
        {
            return new MatchReport()
            {
                MatchRatingStatus = MatchRating.Error,
                LogEntryStatus = LogEntryStatus.CANCELED,
                Reason = reason,
                Income = 0,
                Cost = 0
            };
        }

        public static MatchReport FromHackerAttackSuccessful(string reason)
        {
            return new MatchReport()
            {
                MatchRatingStatus = MatchRating.Error,
                LogEntryStatus = LogEntryStatus.ATTACKED,
                Reason = reason,
                Income = 0,
                Cost = 0
            };
        }

        public static MatchReport FromHackerAttackDefended()
        {
            return new MatchReport()
            {
                MatchRatingStatus = MatchRating.Ignore,
                Income = 0,
                Cost = 0
            };
        }
        #endregion

        public MatchRating MatchRatingStatus
        {
            get;
            private set;
        }

        public LogEntryStatus LogEntryStatus
        {
            get;
            private set;
        }
        
        public string Reason
        {
            get;
            private set;
        }

        public bool HasError => MatchRatingStatus == MatchRating.Error;
        
        /// <summary>
        /// Win or loose only possible if the match was actually
        /// played successfully.
        /// </summary>
        public bool HasWon => MatchRatingStatus == MatchRating.PlayedMatchSuccessfully && Income > 0;

        /// <summary>
        /// Win or loose only possible if the match was actually
        /// played successfully.
        /// </summary>
        public bool HasLost => MatchRatingStatus == MatchRating.PlayedMatchSuccessfully && !HasWon;

        public bool HasLogEntry => Reason != null;
        
        public int Income
        {
            get;
            private set;
        }

        public int Cost
        {
            get;
            private set;
        }
        
        private static int CalculateHumanTeamMoneyOutcome(MatchResponse matchResponse)
        {
            var outcome = FixedMatchStake;
            if (matchResponse.Bet.HasValue)
            {
                outcome = (int)Math.Floor((1 + matchResponse.Bet.Value) * FixedMatchStake);
            }
            
            if (matchResponse.MatchOutcome.HasValue)
            {
                return matchResponse.MatchOutcome.Value switch
                {
                    Outcome.SmoorghWins => -1 * outcome,
                    Outcome.HumanBotWins => outcome,
                    _ => throw new ArgumentOutOfRangeException("matchResponse", $"Unexpected match outcome: {matchResponse.MatchOutcome.Value}")
                };
            }

            return 0;
        }
    }
}