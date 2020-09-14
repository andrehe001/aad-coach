using System;
using AdventureDayRunner.Model;

namespace AdventureDayRunner.Players
{
    public class MatchReport
    {
        private const decimal FixedMatchCosts = 1.0m;

        private MatchReport()
        {
        }

        public static MatchReport FromCostCalculator(decimal monitoredCosts, decimal maximumAllowedCost)
        {
            // TODO: what is the  
            if (monitoredCosts <= maximumAllowedCost)
            {
                return new MatchReport()
                {
                    Status = MatchRating.Success,
                    Reason = $"You are within your Azure Budget limit.",
                    Cost = monitoredCosts
                };
            }
            else
            {

                return new MatchReport()
                {
                    Status = MatchRating.Failed,
                    Reason = $"You surpassed your Azure Budget limit.",
                    Cost = monitoredCosts
                };
            }
        }

        public static MatchReport FromMatchResponse(MatchResponse matchResponse)
        {
            var income = CalculateIncome(matchResponse);
            return new MatchReport()
            {
                Status = MatchRating.Success,
                Reason = $"Match played successfully. You have {(income > 0 ? "won" : "lost")}.",
                Cost = FixedMatchCosts,
                Income = income
            };
        }

        public static MatchReport FromError(string error)
        {
            return new MatchReport()
            {
                Status = MatchRating.Failed,
                Reason = error,
                Cost = FixedMatchCosts
            };
        }
        
        public static MatchReport FromCancellation(string reason)
        {
            return new MatchReport()
            {
                Status = MatchRating.Canceled,
                Reason = reason,
                Cost = FixedMatchCosts
            };
        }


        public static MatchReport FromHackerAttack(bool hasDefendedAttack)
        {
            // A hacker attack neither yields income nor costs?
            if (hasDefendedAttack)
            {
                return new MatchReport()
                {
                    Status = MatchRating.Success, Reason = "Hacker attack successfully defended."
                };
            }
            else
            {
                return new MatchReport()
                {
                    Status = MatchRating.Failed, Reason = "Hacker attack succeeded, your team failed."
                };
            }
        }

        public MatchRating Status
        {
            get;
            protected set;
        }

        public string Reason
        {
            get;
            protected set;
        }

        public bool HasWon => Income > 0;

        public bool HasLost => !HasWon;

        public decimal Income
        {
            get;
            protected set;
        }

        public decimal Cost
        {
            get;
            protected set;
        }
        
        private static decimal CalculateIncome(MatchResponse matchResponse)
        {
            if (matchResponse.MatchOutcome.HasValue)
            {
                return matchResponse.MatchOutcome.Value switch
                {
                    Outcome.ChallengerWins => 0,
                    Outcome.OverlordWins => FixedMatchCosts * 2 * matchResponse.Bet ?? FixedMatchCosts * 2,
                    Outcome.Tie => FixedMatchCosts,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else
            {
                return 1;
            }
        }

    }
}