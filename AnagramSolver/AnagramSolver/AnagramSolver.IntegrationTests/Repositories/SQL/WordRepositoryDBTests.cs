using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.IntegrationTests.Repositories.SQL
{
    [TestFixture]
    public class WordRepositoryDBTests
    {
        WordRepositoryDB _repo;
        SqlConnection _sqlConnection;

        [SetUp]
        public void SetUp()
        {
            string conn = "Data Source=.;Initial Catalog=AnagramSolverTesting;Integrated Security=True";
            Settings.ConnectionStringDevelopment = conn;

            _sqlConnection = new SqlConnection()
            {
                ConnectionString = conn
            };

            _repo = new WordRepositoryDB();

        }
        [TearDown]
        public async Task TearDown()
        {
            //clear all table data
            TableHandler handler = new TableHandler();
            await handler.ClearSelectedTables(new List<string> { "Words" });
        }

        private async Task<WordEntity> GetWordByPhrase(string phrase)
        {
            _sqlConnection.Open();
            SqlCommand cmd = new SqlCommand()
            {
                Connection = _sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from Words where Word = @word"
            };
            cmd.Parameters.Add(new SqlParameter("@word", phrase));
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            var entity = new WordEntity();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    entity =
                        new WordEntity()
                        {
                            ID = int.Parse(reader["ID"].ToString()),
                            Word = reader["Word"].ToString(),
                            Category = reader["Category"].ToString(),
                            SortedWord = reader["SortedWord"].ToString()
                        };
                    break;
                }
            }
            return entity;
        }


        [Test]
        public async Task InsertNewWordSuccess()
        {
            var word = new Anagram() { Category = "dkt", Word = "alus" };

            await _repo.AddNewWord(word);

            var insertedWord = await GetWordByPhrase(word.Word);

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

            var wordEntity = await GetWordByPhrase(word.Word);

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

            var wordEntity = await GetWordByPhrase(word.Word);

            await _repo.UpdateSelectedWord(wordEntity.ID, newWord);

            var updatedWord = await _repo.SelectWordById(wordEntity.ID);

            Assert.AreEqual(newWord.Category, updatedWord.Category);
            Assert.AreEqual(newWord.Word, updatedWord.Word);
        }


        [Test]
        public async Task DeleteSelectedWordSuccess()
        {
            var word = new Anagram() { Category = "dkt", Word = "word" };

            await _repo.AddNewWord(word);
            var allWordsBefore = await _repo.GetAllWords();
            var insertedWord = await GetWordByPhrase(word.Word);

            await _repo.DeleteSelectedWord(insertedWord.ID);

            var allWordsAfter = await _repo.GetAllWords();

            Assert.AreEqual(1, allWordsBefore.Count);
            Assert.AreEqual(0, allWordsAfter.Count);
        }
    }
}
