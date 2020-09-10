using System;
using Serilog;

namespace AdventureDayRunner.Players
{
    public class RandomPlayer : PlayerBase
    {
        public RandomPlayer(string uri) : base(uri)
        {            
        }

        protected override Move GetNextMove(MatchStatistic historicMatchStatistics)
        {            
            Array values = Enum.GetValues(typeof(Move));
            Random random = new Random();
            Move randomMove = (Move)values.GetValue(random.Next(values.Length));
            Log.Information("Generated next move based on randomize strategy: " + randomMove.ToString());
            return randomMove;
        }
    }
}