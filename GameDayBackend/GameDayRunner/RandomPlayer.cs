using System;

namespace GameDayRunner
{
    public class RandomPlayer : Player
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public RandomPlayer(string uri) : base(uri)
        {            
        }

        protected override Move getNextMove(MatchStatistic statisticSoFar)
        {            
            Array values = Enum.GetValues(typeof(Move));
            Random random = new Random();
            Move randomMove = (Move)values.GetValue(random.Next(values.Length));
            Logger.Info("Generated next move based on randomize strategy: " + randomMove.ToString());
            return randomMove;
        }
    }
}