using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.IntegrationTests.Repositories
{
    [TestFixture]
    public class CachedWordRepositoryEFTests
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
            await _context.SaveChangesAsync();

            var insertedWord = await _repo.GetCachedWord(word.SearchPhrase);

            Assert.AreEqual(word.SearchPhrase, insertedWord.Phrase);
            Assert.AreEqual(word.AnagramsIds, insertedWord.AnagramsIds);
        }
    }
}
