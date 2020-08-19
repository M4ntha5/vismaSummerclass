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
    public class CachedWordServiceTests
    {
        ICachedWordRepository _cachedWordRepoMock;
        IMapper _mapperMock;
        ICachedWordService _cachedWordService;

        [SetUp]
        public void Setup()
        {
            _cachedWordRepoMock = Substitute.For<ICachedWordRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _cachedWordService = new CachedWordService(_cachedWordRepoMock, _mapperMock);
        }

        [Test]
        public void FailWhenGettingSelectedCachedWordWithoutPhrase()
        {
            Assert.ThrowsAsync<Exception>(
               async () => await _cachedWordService.GetSelectedCachedWord(null));
        }

        [Test]
        public async Task GetSelectedWordFailedWhenNoWordFound()
        {
            _cachedWordRepoMock.GetCachedWord(Arg.Any<string>()).Returns((CachedWordEntity)null);

            var result = await _cachedWordService.GetSelectedCachedWord("phrase");

            await _cachedWordRepoMock.Received().GetCachedWord(Arg.Any<string>());
            Assert.IsNull(result);
        }

        [Test]
        public async Task SuccessWhenGettingSelectedCachedWord()
        {
            var model = new CachedWord("phrase", "anagrams");
            _cachedWordRepoMock.GetCachedWord(Arg.Any<string>()).Returns(new CachedWordEntity());
            _mapperMock.Map<CachedWord>(Arg.Any<object>()).Returns(model);

            var result = await _cachedWordService.GetSelectedCachedWord("phrase");

            await _cachedWordRepoMock.Received().GetCachedWord(Arg.Any<string>());
            _mapperMock.Received().Map<CachedWord>(Arg.Any<CachedWordEntity>());
            Assert.AreEqual(model.SearchPhrase, result.SearchPhrase);
            Assert.AreEqual(model.AnagramsIds, result.AnagramsIds);
        }
        
        [Test]
        public void FailWhenTryingToAddNewWordWithMandatoryFieldsNotFilled()
        {
            Assert.ThrowsAsync<Exception>(
               async () => await _cachedWordService.AddCachedWord(null, new List<string>()));
        }

        [Test]
        public async Task SuccessWhenTryingToAddNewWordWithMandatoryFieldsFilled()
        {
            var idsList = new List<string>() { "156", "86" };
            await _cachedWordRepoMock.InsertCachedWord(Arg.Any<CachedWord>());

            await _cachedWordService.AddCachedWord("phrase", idsList);

            await _cachedWordRepoMock.Received().InsertCachedWord(Arg.Any<CachedWord>());
        }
    }
}
