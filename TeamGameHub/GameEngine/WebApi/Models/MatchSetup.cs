using System;

namespace TeamGameHub.GameEngine.WebApi.Models
{
    public class MatchSetup
    {
        /// <summary>
        /// The challenger's identifier.
        /// </summary>
        public Guid ChallengerId { get; set; }
        
        /// <summary>
        /// The unique ID of the prepared match.
        /// </summary>
        public Guid MatchId { get; set; }

        /// <summary>
        /// A sequence number for each match that was setup for the provided ChallengerId.
        /// </summary>
        public long MatchSequenceNumber { get; set; }
    }
}