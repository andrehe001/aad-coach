using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using team_management_api.Data.Runner;

namespace team_management_api.Data
{
    public class AdventureDayBackendDbContext : DbContext
    {

        public AdventureDayBackendDbContext(DbContextOptions<AdventureDayBackendDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamLogEntry>()
                .HasKey(_ => new { _.TeamId, _.Id });

            modelBuilder.Entity<TeamLogEntry>()
                .HasIndex(_ => _.Timestamp);
            
            var converter = new ValueConverter<RunnerPhasesConfiguration, string>(
                v => v.ToJson(),
                v => RunnerPhasesConfiguration.FromJson(v));

            modelBuilder
                .Entity<RunnerProperties>()
                .Property(_ => _.PhaseConfigurations)
                .HasConversion(converter)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<RunnerProperties>()
                .HasData(team_management_api.Data.Runner.RunnerProperties.CreateDefault());
        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<TeamScore> TeamScores { get; set; }

        public DbSet<TeamLogEntry> TeamLogEntries { get; set; }
        
        public DbSet<RunnerProperties> RunnerProperties { get; set; }
    }
}