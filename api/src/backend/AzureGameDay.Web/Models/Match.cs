using System;

namespace AzureGameDay.Web.Models
{
    public class Match
    {
        /// <summary>
        /// The ID of the played match.
        /// </summary>
        public Guid MatchId { get; set; }
        
        /// <summary>
        /// A sequence number for each match that was setup for the provided ChallengerId.
        /// </summary>
        public long MatchSequenceNumber { get; set; }
     
        /// <summary>
        /// The challenger's identifier.
        /// </summary>
        public Guid ChallengerId { get; set; }
        
        /// <summary>
        /// The timestamp of the match.
        /// </summary>
        public DateTime WhenUtc { get; set; }
        
        /// <summary>
        /// The move of the challenger.
        /// </summary>
        public Move ChallengerMove { get; set; }
        
        /// <summary>
        /// The move of the overlord.
        /// </summary>
        public Move OverlordMove { get; set; }
        
        /// <summary>
        /// The outcome of the match.
        /// </summary>
        public Outcome Outcome { get; set; }

        public static Match CreateNewFromMatchSetup(MatchSetup matchSetup)
        {
            return new Match()
            {
                MatchId = matchSetup.MatchId,
                ChallengerId = matchSetup.ChallengerId,
                MatchSequenceNumber =  matchSetup.MatchSequenceNumber,
                WhenUtc = DateTime.UtcNow
            };
        }
    }
}