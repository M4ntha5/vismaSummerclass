using AnagramSolver.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Tests
{
    public class AnagramSolverTestingContext : DbContext
    {
        public AnagramSolverTestingContext()
        {
        }

        public AnagramSolverTestingContext(DbContextOptions<AnagramSolverTestingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CachedWordEntity> CachedWords { get; set; }
        public virtual DbSet<UserLogEntity> UserLogs { get; set; }
        public virtual DbSet<WordEntity> Words { get; set; }
    }
}
