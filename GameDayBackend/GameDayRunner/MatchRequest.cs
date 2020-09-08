namespace GameDayRunner
{
    public enum Move
    {
        Rock,
        Paper,
        Scissors,
        Lizard,
        Spock
    }

    internal class MatchRequest
    {
        public string ChallengerId { get; set; }
        public Move Move { get; set; }
    }
}