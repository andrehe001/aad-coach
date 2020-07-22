using System;
using System.Collections.Generic;

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
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        /// <summary>
        /// The moves
        /// </summary>
        public List<Move>  TurnsPlayer1Values { get; set; }
        public List<Move> TurnsPlayer2Values { get; set; }
        

        /// <summary>
        /// Turn of game; first turn is 0; after 3 turns game over;
        /// </summary>
        public int Turn { get; set; }

        /// <summary>
        /// The timestamp of the match.
        /// </summary>
        public DateTime WhenUtc { get; set; }         
        
        
        /// <summary>
        /// Outcome of last round
        /// </summary>
        public Outcome? LastRoundOutcome { get; set; }

        /// <summary>
        /// The outcome of the match.
        /// </summary>
        public Outcome? MatchOutcome { get; set; }

        //public static Match CreateNewFromMatchSetup(MatchSetup matchSetup)
        //{
        //    return new Match()
        //    {
        //        MatchId = matchSetup.MatchId,
        //        Player1Name = matchSetup.ChallengerId,
                
        //        MatchSequenceNumber =  matchSetup.MatchSequenceNumber,
        //        WhenUtc = DateTime.UtcNow
        //    };
        //}

        internal static Match CreateNewFromMatchRequest(MatchRequest matchRequest)
        {
            return new Match()
            {
                MatchId = Guid.NewGuid(),
                Player1Name = matchRequest.ChallengerId.ToString(),
                Player2Name = "bot",
                Turn = 0,
                TurnsPlayer1Values = new List<Move>(),
                TurnsPlayer2Values = new List<Move>(),
                MatchOutcome = null,
                WhenUtc = DateTime.UtcNow
            };
        }
    }
}