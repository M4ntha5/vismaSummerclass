using System;
using AnagramSolver.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AnagramSolver.EF.DatabaseFirst.Data
{
    public partial class AnagramSolverContext : DbContext
    {
        public AnagramSolverContext()
        {
        }

        public AnagramSolverContext(DbContextOptions<AnagramSolverContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CachedWordEntity> CachedWords { get; set; }
        public virtual DbSet<UserLogEntity> UserLogs { get; set; }
        public virtual DbSet<WordEntity> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AnagramSolver3;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CachedWordEntity>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.AnagramsIds)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Phrase)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserLogEntity>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phrase)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<WordEntity>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SortedWord)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Word)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
