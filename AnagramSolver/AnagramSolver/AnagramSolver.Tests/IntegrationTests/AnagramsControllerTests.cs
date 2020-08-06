using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace AnagramSolver.Tests.IntegrationTests
{
    class AnagramsControllerTests
    {
       /* IWordRepository WordRepositoryMock;
        ICookiesHandler CookiesHandlerMock;
        List<Anagram> list;
        Anagram anagram;
        AnagramsController Controller;

        [SetUp]
        public void Setup()
        {
            WordRepositoryMock = Substitute.For<IWordRepository>();
            CookiesHandlerMock = Substitute.For<ICookiesHandler>();

            Controller = new AnagramsController(WordRepositoryMock, CookiesHandlerMock, new WordQueries());

            anagram = new Anagram { Case = "case2", Word = "word2" };
            list = new List<Anagram>()
            {
                new Anagram { Case = "case1", Word = "word1" },
                anagram
            };
        }

        [Test]
        public void GetAllAnagrams()
        {
            WordRepositoryMock.GetWords().Returns(list);

            var result = Controller.Index(1) as ViewResult;
            var data = result.ViewData.Model as PaginatedList<Anagram>;

            WordRepositoryMock.Received().GetWords();
            Assert.AreEqual(list.Count, data.Count);
            Assert.AreEqual(list[0].Word, data[0].Word);

        }

        [Test]
        public void NoAnagramsFound()
        {
            WordRepositoryMock.GetWords().Returns(new List<Anagram>());

            var result = Controller.Index(1) as ViewResult;

            WordRepositoryMock.Received().GetWords();
            Assert.AreEqual(1, result.ViewData.ModelState.ErrorCount);
        }

        [Test]
        public void GetSelectedWordDetails()
        {
            WordRepositoryMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(list);
            
            var result = Controller.Details("abc") as ViewResult;
            var data = result.Model as List<Anagram>;

            WordRepositoryMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            Assert.AreEqual(list.Count, data.Count);
            Assert.AreEqual(list[0].Word, data[0].Word);
        }

        [Test]
        public void SelectedWordDoesNotExistsInDictionary()
        {
            var result = Controller.Details(null) as ViewResult;

            Assert.AreEqual(1, result.ViewData.ModelState.ErrorCount);
        }

        [Test]
        public void NoAnagramsFoundForSelectedWordRedirectToIndex()
        {
            WordRepositoryMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(new List<Anagram>());

            var result = Controller.Details("???") as RedirectToActionResult;

            WordRepositoryMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void NewWordCreatedSuccessfully()
        {
            WordRepositoryMock.AddNewWord(Arg.Any<Anagram>());
            CookiesHandlerMock.ClearAllCookies();

            var result = Controller.Create(anagram) as RedirectToActionResult;

            CookiesHandlerMock.Received().ClearAllCookies();
            WordRepositoryMock.Received().AddNewWord(Arg.Any<Anagram>());
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public void DataNotPresentWhenCreatingNewWord()
        {
            var result = Controller.Create(new Anagram()) as ViewResult;

            Assert.AreEqual(1, result.ViewData.ModelState.ErrorCount);
        }*/
    }
}
