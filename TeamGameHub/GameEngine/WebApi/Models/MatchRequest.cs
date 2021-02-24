using System;

namespace TeamGameHub.GameEngine.WebApi.Models
{
    public class MatchRequest
    {
        /// <summary>
        /// The challenger's identifier.
        /// </summary>
        public string ChallengerId { get; set; }
        
        /// <summary>
        /// The ID of the previously setup match.
        /// </summary>
        public Guid MatchId { get; set; }
               
        /// <summary>
        /// The challenger's move.
        /// </summary>
        public Move Move { get; set; }
    }
}