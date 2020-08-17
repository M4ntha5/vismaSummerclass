using AnagramSolver.Console;
using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using AnagramSolver.EF.CodeFirst;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.Repositories
{
    [TestFixture]
    public class CachedWordReposioryEFTests
    {
        protected AnagramSolverCodeFirstContext _context;
        protected IDbContextTransaction _transaction;

        BusinessLogic.Repositories.CachedWordRepositoryEF _repo;

        [SetUp]
        public void Setup()
        {
            var connStringTesting = "Data Source=.;Initial Catalog=AnagramSolverTesting;Integrated Security=True";
            Settings.ConnectionStringCodeFirst = connStringTesting;

            var options = new DbContextOptionsBuilder<AnagramSolverCodeFirstContext>()
              .UseSqlServer(connStringTesting).Options;

            _context = new AnagramSolverCodeFirstContext(options);
            _context.Database.Migrate();
          
            _repo = new BusinessLogic.Repositories.CachedWordRepositoryEF(_context);

            _context.Database.BeginTransaction();
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.RollbackTransaction();

            /*await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            await _context.DisposeAsync();*/
        }

        [Test]
        public async Task InsertCachedWordSuccess()
        {
            var word = new CachedWord("test-phrase8", "1;2;3");

            await _repo.InsertCachedWord(word);
            
            _context.SaveChanges();

            var item = await _context.CachedWords.FirstOrDefaultAsync(i => i.Phrase == "test-phrase8");
            item.ShouldNotBeNull();
            item.ID.ShouldNotBe(0);
        }

        [Test]
        public async Task GetCahcedWordSuccess()
        {
            var word = new CachedWordEntity() { Phrase = "test-phrase", AnagramsIds = "1;2;3" };
            await _context.AddAsync(word);
            await _context.SaveChangesAsync();

            var item = await _repo.GetCachedWord("test-phrase");
            item.ShouldNotBeNull();
            item.ID.ShouldNotBe(0);
        }
    }
}
