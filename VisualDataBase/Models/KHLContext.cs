using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VisualDataBase
{
    public partial class KHLContext : DbContext
    {
        public KHLContext()
        {
        }

        public KHLContext(DbContextOptions<KHLContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Nation> Nations { get; set; } = null!;
        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<PlayersSeason> PlayersSeasons { get; set; } = null!;
        public virtual DbSet<Season> Seasons { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=D:\\DevelopProjects\\DataBaseViewer\\KHL.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nation>(entity =>
            {
                entity.ToTable("nations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .HasColumnType("STRING")
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("players");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fullname)
                    .HasColumnType("STRING")
                    .HasColumnName("fullname");
            });

            modelBuilder.Entity<PlayersSeason>(entity =>
            {
                entity.HasKey(e => new { e.IdPlayer, e.IdSeason });

                entity.ToTable("players_seasons");

                entity.Property(e => e.IdPlayer).HasColumnName("id_player");

                entity.Property(e => e.IdSeason).HasColumnName("id_season");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Assists).HasColumnName("assists");

                entity.Property(e => e.GamesPlayed).HasColumnName("games_played");

                entity.Property(e => e.GoalsScored).HasColumnName("goals_scored");

                entity.Property(e => e.IdNation).HasColumnName("id_nation");

                entity.Property(e => e.Points).HasColumnName("points");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.HasOne(d => d.IdNationNavigation)
                    .WithMany(p => p.PlayersSeasons)
                    .HasForeignKey(d => d.IdNation);
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.ToTable("seasons");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .HasColumnType("STRING")
                    .HasColumnName("title");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
