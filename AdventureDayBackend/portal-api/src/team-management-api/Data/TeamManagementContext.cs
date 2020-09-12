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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamLogEntry>()
                .HasKey(_ => new { _.TeamId, _.Id });

            modelBuilder.Entity<TeamLogEntry>()
                .HasIndex(_ => _.Timestamp);
        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<TeamScore> TeamScores { get; set; }

        public DbSet<TeamLogEntry> TeamLogEntries { get; set; }
    }
}
