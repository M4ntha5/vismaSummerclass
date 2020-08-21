using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.Services
{
    [TestFixture]
    public class SearchHistoryServiceTests
    {
        ICachedWordService _cachedWordService;
        IWordService _wordServiceMock;

        ISearchHistoryService _historyService;

        [SetUp]
        public void Setup()
        {
            _wordServiceMock = Substitute.For<IWordService>();
            _cachedWordService = Substitute.For<ICachedWordService>();

            _historyService = new SearchHistoryService(_cachedWordService, _wordServiceMock);
        }

        [Test]
        public void GetSearchedAnagramsFailedWhenSearchPhraseNotDefined()
        {
            Assert.ThrowsAsync<Exception>(
                async () => await _historyService.GetSearchedAnagrams(null));
        }

        [Test]
        public async Task GetSearchedAnagramsSuccessWhenSearchPhraseDefined()
        {
            var chachedWord = new CachedWord("phrase", "1;2/8");
            var word = new Anagram() { Category = "cat", Word = "word" };

            _cachedWordService.GetSelectedCachedWord(Arg.Any<string>()).Returns(chachedWord);
            _wordServiceMock.GetWordById(Arg.Any<int>()).Returns(word);

            var result = await _historyService.GetSearchedAnagrams("phrase");

            await _cachedWordService.Received().GetSelectedCachedWord(Arg.Any<string>());
            await _wordServiceMock.Received().GetWordById(Arg.Any<int>());

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(word.Word, result[0]);
            Assert.AreEqual($"{word.Word} {word.Word}", result[1]);
        }


    }
}
