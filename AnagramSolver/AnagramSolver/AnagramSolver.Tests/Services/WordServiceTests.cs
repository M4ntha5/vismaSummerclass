using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.Services
{
    [TestFixture]
    public class WordServiceTests
    {
        IWordRepository _wordRepoMock;
        IAdditionalWordRepository _additionalWordRepoMock;
        IMapper _mapperMock;
        IWordService _wordService;

        [SetUp]
        public void Setup()
        {
            _wordRepoMock = Substitute.For<IWordRepository>();
            _additionalWordRepoMock = Substitute.For<IAdditionalWordRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _wordService = new WordService(_wordRepoMock, _additionalWordRepoMock, _mapperMock);
        }

        [Test]
        public async Task GetAllWordsListSuccessfully()
        {
            var anagrams = new List<Anagram>()
            {
                new Anagram(){Category = "cat", Word = "Word"}
            };
            _wordRepoMock.GetAllWords().Returns(new List<WordEntity>() { new WordEntity() });
            _mapperMock.Map<List<Anagram>>(Arg.Any<List<WordEntity>>()).Returns(anagrams);

            await _wordService.GetAllWords();

            await _wordRepoMock.Received().GetAllWords();
            _mapperMock.Received().Map<List<Anagram>>(Arg.Any<List<WordEntity>>());
        }

        [Test]
        public async Task GetAllWordsListFailedWhenNoWordsFound()
        {
            _wordRepoMock.GetAllWords().Returns(new List<WordEntity>());

            Assert.ThrowsAsync<Exception>(async () => await _wordService.GetAllWords());

            await _wordRepoMock.Received().GetAllWords();
        }

        [Test]
        public async Task GetWordsBySearchSuccessfully()
        {
            var anagrams = new List<Anagram>()
            {
                new Anagram(){Category = "cat", Word = "Word"}
            };
            _additionalWordRepoMock.SelectWordsBySearch(Arg.Any<string>()).Returns(
                new List<WordEntity>() { new WordEntity() });
            _mapperMock.Map<List<Anagram>>(Arg.Any<List<WordEntity>>()).Returns(anagrams);

            var result = await _wordService.GetWordsBySearch("phrase");

            await _additionalWordRepoMock.Received().SelectWordsBySearch(Arg.Any<string>());
            _mapperMock.Received().Map<List<Anagram>>(Arg.Any<List<WordEntity>>());
            Assert.AreEqual(anagrams.Count, result.Count);
            Assert.AreEqual(anagrams[0].Word, result[0].Word);
            Assert.AreEqual(anagrams[0].Category, result[0].Category);
        }

        [Test]
        public void GetWordsBySearchFailedWhenPhraseNotDefined()
        {
            Assert.ThrowsAsync<Exception>(
               async () => await _wordService.GetWordsBySearch(null));
        }

        [Test]
        public async Task GetWordsBySearchFailedWhenPNoWordsFound()
        {
            _additionalWordRepoMock.SelectWordsBySearch(Arg.Any<string>()).Returns(new List<WordEntity>());

            Assert.ThrowsAsync<Exception>(
               async () => await _wordService.GetWordsBySearch("phrase"));

            await _additionalWordRepoMock.Received().SelectWordsBySearch(Arg.Any<string>());
        }

        [Test]
        public void InsertWordFailedWhenAnagramNotDefined()
        {
            Assert.ThrowsAsync<Exception>(
               async () => await _wordService.InsertWord(null));
        }

        [Test]
        public async Task InsertWordSuccessfullWhenAnagramDefinedAndNoDuplicatesFound()
        {
            var anagram = new Anagram() { Category = "cat", Word = "Word" };
            var entities = new List<WordEntity>()
            {
                new WordEntity(){ Word = "word1" }
            };

            _wordRepoMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(entities);
            await _wordRepoMock.AddNewWord(Arg.Any<Anagram>());

            await _wordService.InsertWord(anagram);

            await _wordRepoMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            await _wordRepoMock.Received().AddNewWord(Arg.Any<Anagram>());
        }

        [Test]
        public async Task InsertWordFailedWhenAnagramDefinedButDuplicatesFound()
        {
            var anagram = new Anagram() { Category = "cat", Word = "Word" };
            var entities = new List<WordEntity>()
            {
                new WordEntity(){ Word = "Word" }
            };

            _wordRepoMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(entities);
            await _wordRepoMock.AddNewWord(Arg.Any<Anagram>());

            Assert.ThrowsAsync<Exception>(
                async () => await _wordService.InsertWord(anagram));

            await _wordRepoMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
        }

        [Test]
        public void GetWordAnagramsFailedWhenWordIsNotDefined()
        {
            Assert.ThrowsAsync<Exception>(
                async () => await _wordService.GetWordAnagrams(null));
        }

        [Test]
        public async Task GetWordAnagramsFailedWhenNoAnagramsFound()
        {
            _wordRepoMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(new List<WordEntity>());

            var result = await _wordService.GetWordAnagrams("word");

            await _wordRepoMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetWordAnagramsSuccessWhenWordIsDefined()
        {
            var anagram = new Anagram() { Category = "cat", Word = "Word" };
            var anagramList = new List<Anagram>() { anagram };

            _wordRepoMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(
                new List<WordEntity>() { new WordEntity() });
            _mapperMock.Map<List<Anagram>>(Arg.Any<List<WordEntity>>()).Returns(anagramList);

            var result = await _wordService.GetWordAnagrams("word");

            await _wordRepoMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            _mapperMock.Received().Map<List<Anagram>>(Arg.Any<List<WordEntity>>());
            Assert.AreEqual(anagramList.Count, result.Count);
            Assert.AreEqual(anagramList[0].Word, result[0].Word);
            Assert.AreEqual(anagramList[0].Category, result[0].Category);
        }

        [Test]
        public void GetWordByIDFailedWhenIdNotDefined()
        {
            Assert.ThrowsAsync<Exception>(
                async () => await _wordService.GetWordById(null));
        }
        [Test]
        public async Task GetWordByIDFailedWhenWordWithSuchIdNotFound()
        {
            _additionalWordRepoMock.SelectWordById(Arg.Any<int>()).Returns((WordEntity)null);

            Assert.ThrowsAsync<Exception>(
                async () => await _wordService.GetWordById(5));
            await _additionalWordRepoMock.Received().SelectWordById(Arg.Any<int>());
        }

        [Test]
        public async Task GetWordByIDSuccessWhenAllDataDefinedCorrectly()
        {
            var model = new Anagram() { Category = "cat", Word = "word" };
            _additionalWordRepoMock.SelectWordById(Arg.Any<int>()).Returns(new WordEntity());
            _mapperMock.Map<Anagram>(Arg.Any<WordEntity>()).Returns(model);

            var result = await _wordService.GetWordById(5);

            await _additionalWordRepoMock.Received().SelectWordById(Arg.Any<int>());
            _mapperMock.Received().Map<Anagram>(Arg.Any<WordEntity>());
            Assert.AreEqual(model.Word, result.Word);
            Assert.AreEqual(model.Category, result.Category);
        }

        [Test]
        public void DeleteWordByIDFailedWhenIdIsZeroOrLess()
        {
            Assert.ThrowsAsync<Exception>(
               async () => await _wordService.DeleteWordById(-2));
        }

        [Test]
        public async Task DeleteWordByIDSuccessWhenIdIsMoreThanZero()
        {
            await _additionalWordRepoMock.DeleteSelectedWord(Arg.Any<int>());

            await _wordService.DeleteWordById(2);

            await _additionalWordRepoMock.Received().DeleteSelectedWord(Arg.Any<int>());
        }

        [Test]
        public void UpdateWordFailedWhenIDOrAnagramNotDefined()
        {
            Assert.ThrowsAsync<Exception>(
               async () => await _wordService.UpdateWord(-2, new Anagram()));
        }

        [Test]
        public async Task UpdateWordSuccessWhenIDAndAnagramDefined()
        {
            await _additionalWordRepoMock.UpdateSelectedWord(Arg.Any<int>(), Arg.Any<Anagram>());

            await _wordService.UpdateWord(2, new Anagram { Category = "aca", Word = "wo" });

            await _additionalWordRepoMock.Received().UpdateSelectedWord(Arg.Any<int>(), Arg.Any<Anagram>());
        }
    }
}
