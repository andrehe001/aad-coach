using System;
using System.Collections.Generic;

namespace GameDayRunner
{
    public class MatchStatistic
    {
        /// <summary>
        /// The ID of the played match.
        /// </summary>
        public string MatchId { get; set; }
        
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
        public List<string> TurnsPlayer1Values { get; set; }
        public List<string> TurnsPlayer2Values { get; set; }
        

        /// <summary>
        /// Turn of game; first turn is 0; after 3 turns game over;
        /// </summary>
        public int Turn { get; set; }

        /// <summary>
        /// The timestamp of the match.
        /// </summary>
        public string WhenUtc { get; set; }    

        /// <summary>
        /// The bet of Player2, value of 1 means if player 2 wins, he gets 2x of the score - if he loose Player1 gets 2x of the score
        /// If null the Player2 does not support bets, value needs to be between 0 and 1.
        // /// </summary>
        public Decimal? Bet { get; set; }       
        
        
        /// <summary>
        /// Outcome of last round
        /// </summary>
        public string LastRoundOutcome { get; set; }

        /// <summary>
        /// The outcome of the match.
        /// </summary>
        public string MatchOutcome { get; set; }


    }
}