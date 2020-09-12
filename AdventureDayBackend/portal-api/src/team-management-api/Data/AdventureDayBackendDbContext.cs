using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using team_management_api.Data.Runner;
using team_management_data;

namespace team_management_api.Models
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
            
            var converter = new ValueConverter<AdventureDayPhaseConfiguration, string>(
                v => v.ToJson(),
                v => AdventureDayPhaseConfiguration.FromJson(v));

            modelBuilder
                .Entity<AdventureDayRunnerProperties>()
                .Property(_ => _.PhaseConfigurations)
                .HasConversion(converter)
                .HasColumnType("nvarchar(max)");

            modelBuilder.Entity<AdventureDayRunnerProperties>()
                .HasData(AdventureDayRunnerProperties.CreateDefault("default"));
        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<TeamScore> TeamScores { get; set; }

        public DbSet<TeamLogEntry> TeamLogEntries { get; set; }
        
        public DbSet<AdventureDayRunnerProperties> RunnerProperties { get; set; }
    }
}