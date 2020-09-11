using Microsoft.EntityFrameworkCore;
using team_management_data;

namespace team_management_api.Models
{
    public class TeamManagementContext : DbContext
    {

        public TeamManagementContext(DbContextOptions<TeamManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
    }
}
