using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamGameHub.GameEngine.WebApi.Models
{
    public class Turn
    {
        public int Id { get; set; }
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

        public string Player1Move { get; set; }
        public string Player2Move { get; set; }

        public DateTime WhenUtc { get; set; }
        public int TurnNumber { get; set; }

    }
}
