using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace AnagramSolver.Tests.IntegrationTests
{
    class AnagramsControllerTests
    {
        IWordRepository WordRepositoryMock;
        ICookiesHandler CookiesHandlerMock;
        IHttpContextAccessor HttpContextAccessorMock;
        List<Anagram> list;
        Anagram anagram;
        AnagramsController Controller;

        [SetUp]
        public void Setup()
        {
            WordRepositoryMock = Substitute.For<IWordRepository>();
            CookiesHandlerMock = Substitute.For<ICookiesHandler>();
            HttpContextAccessorMock = Substitute.For<IHttpContextAccessor>();

            Controller = new AnagramsController(WordRepositoryMock, 
                HttpContextAccessorMock, CookiesHandlerMock);

            anagram = new Anagram { Case = "case2", Word = "word2" };
            list = new List<Anagram>()
            {
                new Anagram { Case = "case1", Word = "word1" },
                anagram
            };
        }

        [Test]
        public void TestAnagramsControllerIndex()
        {
            WordRepositoryMock.GetWords().Returns(list);

            var result = Controller.Index(1);

            WordRepositoryMock.Received().GetWords();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestAnagramsControllerIndexWordsCount0()
        {
            WordRepositoryMock.GetWords().Returns(new List<Anagram>());

            Controller.Index(1);

            WordRepositoryMock.Received().GetWords();
            Assert.Greater(Controller.ModelState.ErrorCount, 0);
        }

        [Test]
        public void TestAnagramsControllerDetails()
        {
            WordRepositoryMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(list);
            

            var result = Controller.Details("abc");

            WordRepositoryMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestAnagramsControllerDetailsIdNotDefined()
        {
            Controller.Details("");

            Assert.Greater(Controller.ModelState.ErrorCount, 0);
        }

        [Test]
        public void TestAnagramsControllerDetailsNoAnagramWordsFound()
        {
            WordRepositoryMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(new List<Anagram>());

            var result = Controller.Details("pff");

            WordRepositoryMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void TestAnagramsControllerCreate()
        {
            WordRepositoryMock.AddWordToFile(Arg.Any<Anagram>());
            CookiesHandlerMock.ClearAllCookies();
            var result = Controller.Create(anagram);

            CookiesHandlerMock.Received().ClearAllCookies();
            WordRepositoryMock.Received().AddWordToFile(Arg.Any<Anagram>());
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void TestAnagramsControllerCreateDataNotFilled()
        {
            Controller.Create(new Anagram());

            Assert.Greater(Controller.ModelState.ErrorCount, 0);
        }
    }
}
