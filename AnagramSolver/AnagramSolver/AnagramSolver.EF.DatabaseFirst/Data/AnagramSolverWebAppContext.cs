using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AnagramSolver.EF.DatabaseFirst.Entities;
using AnagramSolver.Contracts.Utils;

namespace AnagramSolver.EF.DatabaseFirst.Data
{
    public class AnagramSolverWebAppContext : DbContext
    {
        public AnagramSolverWebAppContext (DbContextOptions<AnagramSolverWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<WordEntity> Words { get; set; }
        public DbSet<UserLogEntity> UserLogs { get; set; }
        public DbSet<CachedWordEntity> CachedWords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Settings.ConnectionString);
        }
    }
}
