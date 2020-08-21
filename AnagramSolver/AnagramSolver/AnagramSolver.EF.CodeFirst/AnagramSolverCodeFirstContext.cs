using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Utils;
using Microsoft.EntityFrameworkCore;

namespace AnagramSolver.EF.CodeFirst
{
    public class AnagramSolverCodeFirstContext : DbContext
    {
        public AnagramSolverCodeFirstContext()
        {
        }

        public AnagramSolverCodeFirstContext(DbContextOptions<AnagramSolverCodeFirstContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CachedWordEntity> CachedWords { get; set; }
        public virtual DbSet<UserLogEntity> UserLogs { get; set; }
        public virtual DbSet<WordEntity> Words { get; set; }

    }
}
