using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team_management_api.Data
{
    public class TeamScore
    {
        [Key]
        [ForeignKey("Team")]
        public int TeamId { get; set; }

        public Team Team { get; set; }

        [NotMapped]
        public decimal Score => Wins + Profit - Errors;

        public int Wins { get; set; }

        public int Loses { get; set; }

        public int Errors { get; set; }

        [NotMapped]
        public decimal Profit => Income - Costs;

        public decimal Income{ get; set; }

        public decimal Costs { get; set; }
    }
}
