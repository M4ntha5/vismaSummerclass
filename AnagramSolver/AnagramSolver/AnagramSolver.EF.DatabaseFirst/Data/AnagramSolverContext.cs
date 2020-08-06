using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AnagramSolver.Contracts.Utils;
using AnagramSolver.Contracts.Entities;

namespace AnagramSolver.EF.DatabaseFirst.Data
{
    public class AnagramSolverContext : DbContext
    {
        public AnagramSolverContext (DbContextOptions<AnagramSolverContext> options)
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
