using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;


namespace WOTTracker.Models
{
        public class YotTrackerDbContext : DbContext
        {
        // Les entités AVEC clé primaire utilisent DbSet
        public DbSet<AppSettingsHistory> AppSettingsHistory { get; set; }
        public DbSet<WorkSession> WorkSessions { get; set; }
        public DbSet<DownTimeRecord> DownTimeRecords { get; set; }

        public DbSet<Compensation> Compensations { get; set; }


        // On les déclare comme des DbSet normaux.
        // La configuration dans OnModelCreating (HasNoKey) se chargera de la magie.
        public DbSet<FixedPublicHoliday> FixedPublicHolidays { get; set; }
        public DbSet<MovablePublicHoliday> MovablePublicHolidays { get; set; }

        private string _databasePath;

            public YotTrackerDbContext(string databasePath)
            {
                _databasePath = databasePath;
            }

        public YotTrackerDbContext(DbContextOptions<YotTrackerDbContext> options)
    : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlCe($"Data Source={_databasePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Définir les clés primaires pour les tables normales
            modelBuilder.Entity<AppSettingsHistory>().HasKey(e => e.VersionId);
            modelBuilder.Entity<WorkSession>().HasKey(e => e.Id);
            modelBuilder.Entity<DownTimeRecord>().HasKey(e => e.Id);

            // --- LA PARTIE CORRIGÉE POUR LES TABLES SANS CLÉ ---
            // Au lieu de dire "pas de clé", on lui dit que la clé est la COMBINAISON des colonnes.
            // Cela lui permet de suivre les entités sans avoir besoin d'une colonne Id dédiée.

            modelBuilder.Entity<FixedPublicHoliday>(entity =>
            {
                // On définit une clé primaire "composite" sur les colonnes Month ET Day
                entity.HasKey(e => new { e.Month, e.Day });

                // On donne explicitement le nom de la table
                entity.ToTable("FixedPublicHolidays");
            });

            modelBuilder.Entity<MovablePublicHoliday>(entity =>
            {
                // Idem ici
                entity.HasKey(e => new { e.Month, e.Day });

                // On donne explicitement le nom de la table
                entity.ToTable("MovablePublicHolidays");
            });

            // Relation : Une WorkSession peut avoir plusieurs Compensations
            modelBuilder.Entity<Compensation>()
                .HasOne(c => c.WorkSession)
                .WithMany(ws => ws.Compensations)
                .HasForeignKey(c => c.WorkSessionId);

            // Relation : Un DownTimeRecord peut avoir plusieurs Compensations
            modelBuilder.Entity<Compensation>()
                .HasOne(c => c.DownTimeRecord)
                .WithMany(dt => dt.Compensations)
                .HasForeignKey(c => c.DownTimeRecordId);
        }
    }
}
