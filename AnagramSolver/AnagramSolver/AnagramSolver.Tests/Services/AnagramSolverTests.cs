using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Utils;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.Services
{
    [TestFixture]
    public class AnagramSolverTests
    {
        IWordRepository _wordRepoMock;
        ICachedWordService _cachedWordServiceMock;
        IAnagramSolver _anagramSolver;

        [SetUp]
        public void Setup()
        {
            Settings.MinInputLength = 1;
            _wordRepoMock = Substitute.For<IWordRepository>();
            _cachedWordServiceMock = Substitute.For<ICachedWordService>();

            _anagramSolver = new BusinessLogic.Services.AnagramSolver(_wordRepoMock, _cachedWordServiceMock);
        }

        [Test]
        public void GetAnagramsFailedWhenInputDoNotPassValidation()
        {
            Assert.ThrowsAsync<Exception>(
               async () => await _anagramSolver.GetAnagrams(null));
        }
        [Test]
        public void GetAnagramsFailedWhenInputIsLessThanMinimumLength()
        {
            Settings.MinInputLength = 5;
            Assert.ThrowsAsync<Exception>(
               async () => await _anagramSolver.GetAnagrams("1"));
        }

        [Test]
        public async Task GetAnagramsSuccessWhenDataOkAndSingleAndMultipleWordAnagramsFound()
        {
            Settings.AnagramsToGenerate = 5;
            var entity = new WordEntity() { Word = "rokas", SortedWord = "akors" };
            var entity2 = new WordEntity() { Word = "ro", SortedWord = "or" };
            var entity3 = new WordEntity() { Word = "kas", SortedWord = "aks" };
            var list = new List<WordEntity>() { entity, entity2, entity3 };

            _wordRepoMock.GetAllWords().Returns(list);
            _wordRepoMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(new List<WordEntity> { entity });
            await _cachedWordServiceMock.AddCachedWord(Arg.Any<string>(), Arg.Any<List<string>>());

            var result = await _anagramSolver.GetAnagrams("oskar");

            await _wordRepoMock.Received().GetAllWords();
            await _wordRepoMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            await _cachedWordServiceMock.Received().AddCachedWord(Arg.Any<string>(), Arg.Any<List<string>>());

            Assert.AreEqual(list.Count, result.Count);
            Assert.AreEqual(list[0].Word, result[0]);
            Assert.AreEqual($"{list[1].Word} {list[2].Word}", result[1]);
            Assert.AreEqual($"{list[2].Word} {list[1].Word}", result[2]);
        }
    }
}
