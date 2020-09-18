using System.Diagnostics.CodeAnalysis;

namespace AdventureDay.ManagementApi.Data.Runner
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum RunnerPhase
    {
        Phase1_Deployment = 1,
        Phase2_Change = 2,
        Phase3_Monitoring = 3,
        Phase4_Scale = 4,
        Phase5_Security = 5,
        Phase6_Intelligence = 6
    }
}