using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class TournamentApiContext : DbContext
    {
        public TournamentApiContext (DbContextOptions<TournamentApiContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Tournament.Core.Entities.Tournament> Tournament => Set<Tournament.Core.Entities.Tournament>();

        public DbSet<Tournament.Core.Entities.Game> Game => Set<Game>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tournament.Core.Entities.Tournament>().ToTable(nameof(Tournament));
            modelBuilder.Entity<Game>().ToTable(nameof(Game));
        }
    }

}
