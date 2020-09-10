namespace AdventureDayRunner.Players
{
    public class PatternPlayer : PlayerBase
    {
        public PatternPlayer(string uri) : base(uri)
        {
        }

        protected override Move GetNextMove(MatchStatistic historicMatchStatistics)
        {
            return Move.Lizard;
        }
    }
}