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
        AnagramSolverCodeFirstContext _context;
        BusinessLogic.Repositories.CachedWordRepositoryEF _repo;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AnagramSolverCodeFirstContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;

            _context = new AnagramSolverCodeFirstContext(options);
            _repo = new BusinessLogic.Repositories.CachedWordRepositoryEF(_context);
        }


        [Test]
        public async Task InsertAndFetchCachedWord()
        {
            var word = new CachedWord("test-phrase8", "1;2;3");

            await _repo.InsertCachedWord(word);
            _context.SaveChanges();

            var insertedWord = await _repo.GetCachedWord(word.SearchPhrase);

            Assert.AreEqual(word.SearchPhrase, insertedWord.Phrase);
            Assert.AreEqual(word.AnagramsIds, insertedWord.AnagramsIds);
        }


       /* [Test]
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
        }*/
    }
}
