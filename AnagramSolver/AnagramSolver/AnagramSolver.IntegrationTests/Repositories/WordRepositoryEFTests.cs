using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.IntegrationTests.Repositories
{
    [TestFixture]
    public class WordRepositoryEFTests
    {
        AnagramSolverCodeFirstContext _context;
        BusinessLogic.Repositories.WordRepositoryEF _repo;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AnagramSolverCodeFirstContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;

            _context = new AnagramSolverCodeFirstContext(options);
            _repo = new BusinessLogic.Repositories.WordRepositoryEF(_context);
        }


        [Test]
        public async Task InsertNewWordSuccess()
        {
            var word = new Anagram() { Category = "dkt", Word = "alus" };

            await _repo.AddNewWord(word);
            await _context.SaveChangesAsync();

            var insertedWord = await _context.Words.Where(x => x.Word == word.Word).FirstOrDefaultAsync();

            Assert.AreEqual(word.Category, insertedWord.Category);
            Assert.AreEqual(word.Word, insertedWord.Word);
            Assert.AreEqual(String.Concat(word.Word.OrderBy(x => x)), insertedWord.SortedWord);
        }

        [Test]
        public async Task GetAllWordsWhen2WordsTotal()
        {
            var word = new Anagram() { Category = "dkt", Word = "alus" };
            var word2 = new Anagram() { Category = "dkt", Word = "oskaras" };

            await _repo.AddNewWord(word);
            await _repo.AddNewWord(word2);
            await _context.SaveChangesAsync();

            var allWords = await _repo.GetAllWords();

            Assert.AreEqual(2, allWords.Count);
            Assert.AreEqual(word.Word, allWords[0].Word);
            Assert.AreEqual(word2.Word, allWords[1].Word);
        }

        [Test]
        public async Task GetSelectedWordAnagramsWhenOneAnagramFound()
        {
            var word = new Anagram() { Category = "dkt", Word = "rokas" };
            var word2 = new Anagram() { Category = "dkt", Word = "oskar" };

            await _repo.AddNewWord(word);
            await _repo.AddNewWord(word2);
            await _context.SaveChangesAsync();

            var anagrams = await _repo.GetSelectedWordAnagrams(word.Word);

            Assert.AreEqual(2, anagrams.Count);
            Assert.AreEqual(word.Word, anagrams[0].Word);
            Assert.AreEqual(word2.Word, anagrams[1].Word);
        }

        [Test]
        public async Task SelectWordById()
        {
            var word = new Anagram() { Category = "dkt", Word = "oskarasss" };

            await _repo.AddNewWord(word);
            await _context.SaveChangesAsync();

            var wordEntity = await _context.Words.Where(x => x.Word == word.Word).SingleOrDefaultAsync();

            var selectedWord = await _repo.SelectWordById(wordEntity.ID);

            Assert.AreEqual(wordEntity.ID, selectedWord.ID);
            Assert.AreEqual(word.Word, selectedWord.Word);
            Assert.AreEqual(word.Category, selectedWord.Category);
        }

        [Test]
        public async Task SelectWordByIdFailedWhenIdNotFound()
        {
            var word = new Anagram() { Category = "dkt", Word = "oskar" };

            await _repo.AddNewWord(word);
            await _context.SaveChangesAsync();

            var selectedWord = await _repo.SelectWordById(95);

            Assert.IsNull(selectedWord);
        }

        [Test]
        public async Task SelectWordsBySearchWhen2WordsFound()
        {
            var word = new Anagram() { Category = "dkt", Word = "kaunas" };
            var word2 = new Anagram() { Category = "dkt", Word = "kaunietis" };

            await _repo.AddNewWord(word);
            await _repo.AddNewWord(word2);
            await _context.SaveChangesAsync();

            var wordsFound = await _repo.SelectWordsBySearch("kau");

            Assert.AreEqual(2, wordsFound.Count);
            Assert.AreEqual(word.Word, wordsFound[0].Word);
            Assert.AreEqual(word.Category, wordsFound[0].Category);
            Assert.AreEqual(word2.Word, wordsFound[1].Word);
            Assert.AreEqual(word2.Category, wordsFound[1].Category);
        }

        [Test]
        public async Task UpdateSelectedWordSuccess()
        {
            var word = new Anagram() { Category = "dkt", Word = "my-word" };
            var newWord = new Anagram() { Category = "updated", Word = "updated" };

            await _repo.AddNewWord(word);
            await _context.SaveChangesAsync();

            var wordEntity = await _context.Words.Where(x => x.Word == word.Word).SingleOrDefaultAsync();

            await _repo.UpdateSelectedWord(wordEntity.ID, newWord);
            await _context.SaveChangesAsync();

            var updatedWord = await _repo.SelectWordById(wordEntity.ID);

            Assert.AreEqual(newWord.Category, updatedWord.Category);
            Assert.AreEqual(newWord.Word, updatedWord.Word);
        }

        [Test]
        public void UpdateSelectedWordFailedWhenSelectedWordDoesNotExist()
        {
            var newWord = new Anagram() { Category = "updated dkt", Word = "updated word" };

            Assert.ThrowsAsync<Exception>(async () => await _repo.UpdateSelectedWord(684, newWord));
        }

        [Test]
        public async Task DeleteSelectedWordSuccess()
        {
            var word = new Anagram() { Category = "dkt", Word = "word" };

            await _repo.AddNewWord(word);
            await _context.SaveChangesAsync();

            var allWordsBefore = await _context.Words.ToListAsync();

            await _repo.DeleteSelectedWord(allWordsBefore[0].ID);
            await _context.SaveChangesAsync();

            var allWordsAfter = await _context.Words.ToListAsync();

            Assert.AreEqual(1, allWordsBefore.Count);
            Assert.AreEqual(0, allWordsAfter.Count);
        }

        [Test]
        public void DeleteSelectedWordFailedWordNotFound()
        {
            Assert.ThrowsAsync<Exception>(async () => await _repo.DeleteSelectedWord(4));
        }
    }
}
