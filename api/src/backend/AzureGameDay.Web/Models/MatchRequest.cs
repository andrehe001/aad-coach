using System;

namespace AzureGameDay.Web.Models
{
    public class MatchRequest
    {
        /// <summary>
        /// The challenger's identifier.
        /// </summary>
        public Guid ChallengerId { get; set; }
        
        /// <summary>
        /// The ID of the previously setup match.
        /// </summary>
        public Guid MatchId { get; set; }
        
        /// <summary>
        /// A sequence number for each match that was setup for the provided ChallengerId.
        /// </summary>
        public long MatchSequenceNumber { get; set; }
        
        /// <summary>
        /// The challenger's move.
        /// </summary>
        public Move Move { get; set; }
    }
}