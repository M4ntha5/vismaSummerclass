using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.Controllers
{
    [TestFixture]
    public class AnagramsControllerTests
    {
        ICookiesHandlerService _cookiesHandlerServiceMock;
        IUserLogService _userLogServiceMock;
        IWordService _wordServiceMock;
        AnagramSolverCodeFirstContext _contextMock;

        AnagramsController _controller;

        List<Anagram> _anagramsList;
        Anagram _anagram;

        [SetUp]
        public void Setup()
        {
            _cookiesHandlerServiceMock = Substitute.For<ICookiesHandlerService>();
            _userLogServiceMock = Substitute.For<IUserLogService>();
            _wordServiceMock = Substitute.For<IWordService>();
            _contextMock = Substitute.For<AnagramSolverCodeFirstContext>();

            _controller = new AnagramsController(
                _cookiesHandlerServiceMock, _wordServiceMock, _userLogServiceMock, _contextMock);

            _anagram = new Anagram() { Word = "word1", ID = 1 };
            _anagramsList = new List<Anagram>() { _anagram };
        }

        [Test]
        public async Task IndexSuccessWhenSearchPhraseEntered()
        {
            _wordServiceMock.GetWordsBySearch(Arg.Any<string>()).Returns(_anagramsList);

            var result = await _controller.Index(1, "phrase") as ViewResult;
            var data = result.ViewData.Model as PaginatedList<Anagram>;

            await _wordServiceMock.Received().GetWordsBySearch(Arg.Any<string>());
            Assert.AreEqual(_anagramsList.Count, data.Count);
            Assert.AreEqual(_anagramsList[0].Word, data[0].Word);
        }

        [Test]
        public async Task IndexSuccessWhenNoSearchPhraseEntered()
        {
            _wordServiceMock.GetAllWords().Returns(_anagramsList);

            var result = await _controller.Index(1) as ViewResult;
            var data = result.ViewData.Model as PaginatedList<Anagram>;

            await _wordServiceMock.Received().GetAllWords();
            Assert.AreEqual(_anagramsList.Count, data.Count);
            Assert.AreEqual(_anagramsList[0].Word, data[0].Word);
        }

        [Test]
        public async Task DetailsFailedWhenIdPhraseNotFound()
        { 
            var result = await _controller.Details(null) as ViewResult;
            var data = result.ViewData.Model as List<Anagram>;

            Assert.IsNull(data);
        }

        [Test]
        public async Task DetailsSuccessWhenNoAnagramsFound()
        {
            _wordServiceMock.GetWordAnagrams(Arg.Any<string>()).Returns((List<Anagram>)null);

            var result = await _controller.Details("phrase") as RedirectToActionResult;

            await _wordServiceMock.Received().GetWordAnagrams(Arg.Any<string>());
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task DetailsSuccessWhenSomeAnagramsFound()
        {
            _wordServiceMock.GetWordAnagrams(Arg.Any<string>()).Returns(_anagramsList);

            var result = await _controller.Details("phrase") as ViewResult;
            var data = result.ViewData.Model as List<Anagram>;

            await _wordServiceMock.Received().GetWordAnagrams(Arg.Any<string>());

            Assert.AreEqual(_anagramsList.Count, data.Count);
            Assert.AreEqual(_anagramsList[0].Word, data[0].Word);
        }

        [Test]
        public void CreateReturnsEmptyView()
        {
            var result = _controller.Create() as ViewResult;
            var data = result.ViewData.Model as List<Anagram>;

            Assert.IsNull(data);
        }

        [Test]
        public async Task CreateFailedWhenMandatoryFieldsNotFilled()
        {
            var result = await _controller.Create(new Anagram()) as ViewResult;
            var data = result.ViewData.Values;

            Assert.AreEqual(1, data.Count);
        }

        [Test]
        public async Task CreateSuccessWhenAllMandatoryFieldsFilled()
        {
            _cookiesHandlerServiceMock.ClearAllCookies();
            await _wordServiceMock.InsertWord(Arg.Any<Anagram>());
            await _userLogServiceMock.AddLog(Arg.Any<TimeSpan>(), Arg.Any<UserActionTypes>());
            await _contextMock.SaveChangesAsync();

            var result = await _controller.Create(_anagram) as RedirectToActionResult;

            _cookiesHandlerServiceMock.Received().ClearAllCookies();
            await _wordServiceMock.Received().InsertWord(Arg.Any<Anagram>());
            await _userLogServiceMock.Received().AddLog(Arg.Any<TimeSpan>(), Arg.Any<UserActionTypes>());
            await _contextMock.Received().SaveChangesAsync();
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task UpdateFormFailedWhenIdIsNotDefined()
        {
            var result = await _controller.Update(null) as ViewResult;
            var data = result.ViewData.Model as List<Anagram>;

            Assert.IsNull(data);
        }
        [Test]
        public async Task UpdateFormFailedWhenWordWithSuchIdNotFound()
        {
            _wordServiceMock.GetWordById(Arg.Any<int>()).Returns((Anagram)null);

            var result = await _controller.Update(5) as ViewResult;
            var data = result.ViewData.Model as List<Anagram>;

            Assert.IsNull(data);
        }

        [Test]
        public async Task UpdateFormSuccessWhenIdDefinedAndWordFound()
        {
            _wordServiceMock.GetWordById(Arg.Any<int>()).Returns(_anagram);

            var result = await _controller.Update(5) as ViewResult;
            var data = result.ViewData.Model as Anagram;

            Assert.AreEqual(_anagram.Word, data.Word);
        }

        [Test]
        public async Task UpdateFailedWhenIdsDoNotMatch()
        {
            var result = await _controller.Update(5, _anagram) as ViewResult;
            var data = result.ViewData.Model as Anagram;

            Assert.AreEqual(_anagram, data);
        }

        [Test]
        public async Task UpdateSuccessWhenAllDataFilledCorrectly()
        {
            await _wordServiceMock.UpdateWord(Arg.Any<int>(), Arg.Any<Anagram>());
            await _userLogServiceMock.AddLog(Arg.Any<TimeSpan>(), Arg.Any<UserActionTypes>());
            await _contextMock.SaveChangesAsync();

            var result = await _controller.Update(1, _anagram) as RedirectToActionResult;

            await _wordServiceMock.Received().UpdateWord(Arg.Any<int>(), Arg.Any<Anagram>());
            await _userLogServiceMock.Received().AddLog(
                Arg.Any<TimeSpan>(), Arg.Any<UserActionTypes>(), Arg.Any<string>());
            await _contextMock.Received().SaveChangesAsync();
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task DeleteFailedWhenIdIsNotDefined()
        {
            var result = await _controller.Delete(null) as ViewResult;
            var data = result.ViewData.Model;

            Assert.IsNull(data);
        }

        [Test]
        public async Task DeleteSuccessWhenIdDefiinedAndValid()
        {
            await _wordServiceMock.DeleteWordById(Arg.Any<int>());
            await _userLogServiceMock.AddLog(Arg.Any<TimeSpan>(), Arg.Any<UserActionTypes>());
            await _contextMock.SaveChangesAsync();

            var result = await _controller.Delete(5) as RedirectToActionResult;

            await _wordServiceMock.Received().DeleteWordById(Arg.Any<int>());
            await _userLogServiceMock.Received().AddLog(Arg.Any<TimeSpan>(), Arg.Any<UserActionTypes>());
            await _contextMock.Received().SaveChangesAsync();
            Assert.AreEqual("Index", result.ActionName);
        }

    }
}
