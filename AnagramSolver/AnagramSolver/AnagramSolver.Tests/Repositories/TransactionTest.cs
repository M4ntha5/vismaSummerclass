using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Utils;
using AnagramSolver.EF.CodeFirst;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Tests.Repositories
{
    public class TransactionTest
    {
        protected DbContextOptions<AnagramSolverCodeFirstContext> ContextOptions { get; }

        protected TransactionTest(DbContextOptions<AnagramSolverCodeFirstContext> contextOptions)
        {
            ContextOptions = contextOptions;

            var connStringTesting = "Data Source=.;Initial Catalog=AnagramSolverTesting;Integrated Security=True";
            Settings.ConnectionStringCodeFirst = connStringTesting;

            Seed();
        }

        private void Seed()
        {
            using (var context = new AnagramSolverCodeFirstContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var one = new WordEntity() { Category = "cat1", Word = "hello1", SortedWord = "ehllo1" };
                var two = new WordEntity() { Category = "cat2", Word = "hello2", SortedWord = "ehllo2" };
                var three = new WordEntity() { Category = "cat3", Word = "hello3", SortedWord = "ehllo3" };

                context.Words.AddRange(one, two, three);

                var one1 = new CachedWordEntity() { Phrase = "phrase1", AnagramsIds = "1;2/3" };
                var two2 = new CachedWordEntity() { Phrase = "phrase2", AnagramsIds = "1;2;3/1" };
                var three3 = new CachedWordEntity() { Phrase = "phrase3", AnagramsIds = "1/2;3" };

                context.CachedWords.AddRange(one1, two2, three3);

                var one11 = new UserLogEntity() { Phrase = "phrase1", Ip = "1", Action = UserActionTypes.GetAnagrams.ToString(), SearchTime = TimeSpan.FromSeconds(2) };
                var two22 = new UserLogEntity() { Phrase = "phrase2", Ip = "12", Action = UserActionTypes.InsertWord.ToString(), SearchTime = TimeSpan.FromSeconds(1) }; ;
                var three33 = new UserLogEntity() { Phrase = "phrase3", Ip = "123", Action = UserActionTypes.DeleteWord.ToString(), SearchTime = TimeSpan.FromSeconds(6) };

                context.UserLogs.AddRange(one11, two22, three33);

                context.SaveChanges();
            }
        }
    }
}
