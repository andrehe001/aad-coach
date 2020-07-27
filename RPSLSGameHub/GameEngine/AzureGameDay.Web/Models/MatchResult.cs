using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RPSLSGameHub.GameEngine.WebApi.Models
{
    public class MatchResult
    {

        /// <summary>
        /// The ID of the match result in the DB - not to be confused with the MatchID
        /// </summary>
        [Key]
        public int Id { get; set; }

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
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }


        
        /// <summary>
        /// The timestamp of the match.
        /// </summary>
        public DateTime WhenUtc { get; set; }


        /// <summary>
        /// The outcome of the match.
        /// </summary>
        public string? MatchOutcome { get; set; }
    }
}
