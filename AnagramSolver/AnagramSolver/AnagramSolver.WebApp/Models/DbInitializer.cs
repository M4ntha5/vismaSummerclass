using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.EF.CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.WebApp.Models
{
    public class DbInitializer
    {
        public static async Task Initialize(AnagramSolverCodeFirstContext context)
        {
            context.Database.EnsureCreated();
        
            // Look for any students.
            if (context.Words.Any())
            {
                return;   // DB has been seeded
            }

            var repo = new FileRepository();

            var words = await repo.GetAllWords();

            context.Words.AddRange(words);
            context.SaveChanges();

        }
    }
}
