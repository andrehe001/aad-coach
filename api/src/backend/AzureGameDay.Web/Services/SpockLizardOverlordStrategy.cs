using System;
using System.Collections.Generic;
using AzureGameDay.Web.Models;

namespace AzureGameDay.Web.Services
{
    public class SpockLizardOverlordStrategy : IOverlordStrategy
    {
        private readonly Dictionary<Guid, Move> _lastMoves = new Dictionary<Guid, Move>();
        
        public Move NextMove(Guid challengerId)
        {
            var foundLastMove = _lastMoves.TryGetValue(challengerId, out var lastMove);
            
            if (!foundLastMove)
            {
                lastMove = Move.Lizard;
            }

            var newMove = lastMove switch
            {
                Move.Lizard => Move.Spock,
                Move.Spock => Move.Lizard,
                _ => Move.Lizard
            };

            _lastMoves[challengerId] = newMove;

            return newMove;
        }
    }
}