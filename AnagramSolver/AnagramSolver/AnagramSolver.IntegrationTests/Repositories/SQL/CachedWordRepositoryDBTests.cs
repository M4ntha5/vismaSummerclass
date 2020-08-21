using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.IntegrationTests.Repositories.SQL
{
    [TestFixture]
    public class CachedWordRepositoryDBTests
    {
        CachedWordRepositoryDB _repo;

        [SetUp]
        public void SetUp()
        {
            var conn = "Data Source=.;Initial Catalog=AnagramSolverTesting;Integrated Security=True";
            Settings.ConnectionStringDevelopment = conn;

            _repo = new CachedWordRepositoryDB();
        }

        [TearDown]
        public async Task TearDown()
        {
            //clear all table data
            TableHandler handler = new TableHandler();
            await handler.ClearSelectedTables(new List<string> { "CachedWords" });
        }

        [Test]
        public async Task InsertAndFetchCachedWord()
        {
            var word = new CachedWord("test-phrase8", "1;2;3");

            await _repo.InsertCachedWord(word);

            var insertedWord = await _repo.GetCachedWord(word.SearchPhrase);

            Assert.AreEqual(word.SearchPhrase, insertedWord.Phrase);
            Assert.AreEqual(word.AnagramsIds, insertedWord.AnagramsIds);
        }
    }
}
