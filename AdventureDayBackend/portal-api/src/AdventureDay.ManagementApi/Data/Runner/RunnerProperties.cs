using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureDay.ManagementApi.Data.Runner
{
    [Table("RunnerProperties")]
    public class RunnerProperties
    {
        public const string DefaultRunnerPropertiesName = "default";
        
        public static RunnerProperties CreateDefault()
        {
            return new RunnerProperties()
            {
                Id = 1,
                Name = DefaultRunnerPropertiesName,
                RunnerStatus = RunnerStatus.Stopped,
                PhaseConfigurations = DefaultRunnerPhasesConfiguration.DefaultConfiguration,
                CurrentPhase = RunnerPhase.Phase1_Deployment
            };
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Column(TypeName = "nvarchar(255)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public RunnerStatus RunnerStatus { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public RunnerPhase CurrentPhase { get; set; }
        
        // Uses JSON converter.
        public RunnerPhasesConfiguration PhaseConfigurations { get; set; }
    }
}