using System;
using System.Configuration;
using System.Text.RegularExpressions;
using AdventureDay.DataModel;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Players
{
    public class MatchReport
    {
        // TODO: Refactor to some more intelligent approach... (=> avoid static ref here)
        private static readonly int FixedMatchStake = Program.Configuration?.GetValue("FixedMatchStake", 1) ?? 1;

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
                ? $"Human has won {outcome} against {matchResponse.Player1Name}"
                : $"Smoorgh ({matchResponse.Player1Name}) has won {Math.Abs(outcome)}";

            return new MatchReport()
            {
                MatchRatingStatus = MatchRating.PlayedMatchSuccessfully,
                LogEntryStatus = LogEntryStatus.SUCCESS,
                Reason = reason,
                Income = outcome > 0 ? outcome : 0,
                Cost =   Math.Abs(outcome > 0 ? 0 : outcome)
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
        public static MatchReport FromBackendUriMissing(string reason)
        {
            return new MatchReport()
            {
                MatchRatingStatus = MatchRating.Ignore,
                LogEntryStatus = LogEntryStatus.CANCELED,
                Reason = reason,
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
                // Bet: Value between 0 and 1.
                if (matchResponse.Bet < 0.0m)
                {
                    matchResponse.Bet = 0.0m;
                }

                if (matchResponse.Bet > 1.0m)
                {
                    matchResponse.Bet = 1.0m;
                }
                
                var additionalBetOutcome = (int)Math.Round((1 + matchResponse.Bet.Value) * FixedMatchStake);
                outcome = FixedMatchStake + additionalBetOutcome;
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