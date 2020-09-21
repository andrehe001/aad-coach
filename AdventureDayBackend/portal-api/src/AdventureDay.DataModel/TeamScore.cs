using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureDay.ManagementApi.Data;

namespace AdventureDay.DataModel
{
    public class TeamScore
    {
        [Key]
        [ForeignKey("Team")]
        public int TeamId { get; set; }

        public Team Team { get; set; }

        [NotMapped]
        public int Score => Wins + Profit - Errors;

        public int Wins { get; set; }

        public int Loses { get; set; }

        public int Errors { get; set; }

        [NotMapped]
        public int Profit => Income - Costs;

        public int Income{ get; set; }

        public int Costs { get; set; }
    }
}
