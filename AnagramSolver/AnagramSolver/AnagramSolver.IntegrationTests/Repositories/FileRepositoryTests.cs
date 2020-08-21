using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.IntegrationTests.Repositories
{
    [TestFixture]
    public class FileRepositoryTests
    {
        IWordRepository _repo;
        [SetUp]
        public void SetUp()
        {
            Settings.DataFileName = "/AnagramSolver.Contracts/DataFiles/zodynas.txt";

            _repo = new FileRepository();
        }

        [Test]
        public async Task GetSelectedWordAnagramsSuccessWhen2AnagramsFound()
        {
            var result = await _repo.GetSelectedWordAnagrams("alus");

            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetSelectedWordAnagramsFailedWhenNoAnagramsFound()
        {
            var result = await _repo.GetSelectedWordAnagrams("asfaf");

            Assert.IsNull(result);
        }

        [Test]
        public void AddNewWordFailedWhenFileDoesNotExist()
        {
            Settings.DataFileName = "";

            Assert.ThrowsAsync<Exception>(async () => await _repo.AddNewWord(new Anagram()));
        }

        [Test]
        public void AddNewWordFailedWhenAnagramIsEmpty()
        {
            Assert.ThrowsAsync<Exception>(async () => await _repo.AddNewWord(new Anagram()));
        }

        [Test]
        public void AddNewWordFailedWhenDuplicateFound()
        {
            var anagram = new Anagram() { Category = "cat", Word = "alus" };

            Assert.ThrowsAsync<Exception>(async () => await _repo.AddNewWord(anagram));
        }

        [Test]
        public async Task AddNewWordSuccess()
        {
            var anagram = new Anagram() { Category = "cat", Word = "test-word" };

            var allWordsBefore = await _repo.GetAllWords();

            await _repo.AddNewWord(anagram);

            var allWordsAfter = await _repo.GetAllWords();

            Assert.AreEqual(allWordsBefore.Count + 1, allWordsAfter.Count);
            //if assertion successfull removes inserted word from file
            await RemoveLastAddedWord();
        }

        [Test]
        public async Task GetAllWordsSuccess()
        {
            var allWords = await _repo.GetAllWords();

            Assert.Greater(allWords.Count, 10);
        }

        private async Task RemoveLastAddedWord()
        {
            //removes added word
            string orgPath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\" + Settings.DataFileName));
            var allData = File.ReadAllLines(orgPath).ToList();
            allData.RemoveAt(allData.Count - 1);
            await File.WriteAllLinesAsync(orgPath, allData.ToArray());
        }

    }
}
