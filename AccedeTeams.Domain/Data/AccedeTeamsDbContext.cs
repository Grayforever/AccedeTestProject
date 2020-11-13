using Cofoundry.Core;
using Cofoundry.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace AccedeTeams.Data
{
    public class AccedeTeamsDbContext : DbContext
    {
        private readonly DatabaseSettings _databaseSettings;

        public AccedeTeamsDbContext(DatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_databaseSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAppSchema()
                .MapCofoundryContent()
                .ApplyConfiguration(new PlayerLikeMap())
                .ApplyConfiguration(new PlayerLikeCountMap());
        }

        public DbSet<PlayerLike> PlayerLikes { get; set; }
        public DbSet<PlayerLikeCount> PlayerLikeCounts { get; set; }
    }
}