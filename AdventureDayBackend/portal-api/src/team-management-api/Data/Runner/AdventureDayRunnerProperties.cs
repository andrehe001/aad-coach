using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace team_management_api.Data.Runner
{
    [Table("RunnerProperties")]
    public class AdventureDayRunnerProperties
    {
        public static AdventureDayRunnerProperties CreateDefault(string name)
        {
            return new AdventureDayRunnerProperties()
            {
                Id = 1,
                Name = name,
                AdventureDayRunnerStatus = AdventureDayRunnerStatus.Stopped,
                PhaseConfigurations = DefaultAdventureDayPhaseConfiguration.DefaultConfiguration,
                CurrentPhase = AdventureDayPhase.Phase1_Deployment
            };
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Column(TypeName = "nvarchar(255)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public AdventureDayRunnerStatus AdventureDayRunnerStatus { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public AdventureDayPhase CurrentPhase { get; set; }
        
        // Uses JSON converter.
        public AdventureDayPhaseConfiguration PhaseConfigurations { get; set; }
    }
}