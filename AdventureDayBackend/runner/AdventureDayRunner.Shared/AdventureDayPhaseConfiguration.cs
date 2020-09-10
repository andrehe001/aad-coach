namespace AdventureDayRunner.Shared
{
    public class AdventureDayPhaseConfiguration
    {
        public int NumberOfRequestExecutorsPerTeam { get; set; }
        public int RequestExecutorLatencyMillis { get; set; }
        public PlayerType[] PlayerTypes { get; set; }
    }
}