using System;
using AdventureDayRunner.Model;

namespace AdventureDayRunner.Players
{
    public class MatchReport
    {
        private const int FixedMatchStake = 1;

        private MatchReport()
        {
        }

        #region Creation Helpers
        public static MatchReport FromCostCalculator(int cost)
        {
            return new MatchReport()
            {
                Status = MatchRating.Ignore,
                Cost = cost
            };
        }

        public static MatchReport FromMatchResponse(MatchResponse matchResponse)
        {
            var income = CalculateIncome(matchResponse);
            return new MatchReport()
            {
                Status = MatchRating.Success,
                Cost = income > 0 ? 0 : FixedMatchStake,
                Income = income > 0 ? income : 0
            };
        }

        public static MatchReport FromError(string error)
        {
            return new MatchReport()
            {
                Status = MatchRating.Failed,
                Reason = error,
                Cost = FixedMatchStake
            };
        }
        
        public static MatchReport FromCancellation(string reason)
        {
            return new MatchReport()
            {
                Status = MatchRating.Canceled,
                Reason = reason,
                Cost = FixedMatchStake
            };
        }

        public static MatchReport FromHackerAttack(bool hasDefendedAttack)
        {
            // A hacker attack neither yields income nor costs?
            if (hasDefendedAttack)
            {
                return new MatchReport()
                {
                    Status = MatchRating.Ignore
                };
            }
            else
            {
                return new MatchReport()
                {
                    Status = MatchRating.Failed,
                    Reason = "Hacker infiltration succeeded, your team is under attack."
                };
            }
        }
        #endregion

        public MatchRating Status
        {
            get;
            private set;
        }

        public string Reason
        {
            get;
            private set;
        }

        public bool HasWon => Income > 0;

        public bool HasLost => !HasWon;

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
        
        private static int CalculateIncome(MatchResponse matchResponse)
        {
            var potentialIncome = FixedMatchStake;
            if (matchResponse.Bet.HasValue)
            {
                potentialIncome += (int)Math.Floor(matchResponse.Bet.Value);
            }
            
            if (matchResponse.MatchOutcome.HasValue)
            {
                return matchResponse.MatchOutcome.Value switch
                {
                    Outcome.ChallengerWins => 0,
                    Outcome.OverlordWins => potentialIncome,
                    _ => throw new ArgumentOutOfRangeException("matchResponse", $"Unexpected match outcome: {matchResponse.MatchOutcome.Value}")
                };
            }

            return 0;
        }
    }
}