using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace AnagramSolver.Tests.IntegrationTests
{
    class AnagramsControllerTests
    {
        IWordRepository WordRepositoryMock;
        List<Anagram> list;
        Anagram anagram;

        [SetUp]
        public void Setup()
        {
            WordRepositoryMock = Substitute.For<IWordRepository>();

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
            var controller = new AnagramsController(WordRepositoryMock);

            var result = controller.Index(1);

            WordRepositoryMock.Received().GetWords();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestAnagramsControllerWordsCount0()
        {
            WordRepositoryMock.GetWords().Returns(new List<Anagram>());
            var controller = new AnagramsController(WordRepositoryMock);

            controller.Index(1);

            WordRepositoryMock.Received().GetWords();
            Assert.Greater(controller.ModelState.ErrorCount, 0);
        }

        [Test]
        public void TestAnagramsControllerDetails()
        {
            WordRepositoryMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(list);
            var controller = new AnagramsController(WordRepositoryMock);

            var result = controller.Details("abc");

            WordRepositoryMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestAnagramsControllerDetailsIdNotDefined()
        {
            var controller = new AnagramsController(WordRepositoryMock);

            controller.Details("");

            Assert.Greater(controller.ModelState.ErrorCount, 0);
        }

        [Test]
        public void TestAnagramsControllerDetailsNoAnagramWordsFound()
        {
            WordRepositoryMock.GetSelectedWordAnagrams(Arg.Any<string>()).Returns(new List<Anagram>());
            var controller = new AnagramsController(WordRepositoryMock);

            var result = controller.Details("pff");

            WordRepositoryMock.Received().GetSelectedWordAnagrams(Arg.Any<string>());
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void TestAnagramsControllerCreate()
        {
            WordRepositoryMock.AddWordToFile(Arg.Any<Anagram>());
            var controller = new AnagramsController(WordRepositoryMock);

            var result = controller.Create(anagram);

            WordRepositoryMock.Received().AddWordToFile(Arg.Any<Anagram>());
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public void TestAnagramsControllerCreateDataNotFilled()
        {
            var controller = new AnagramsController(WordRepositoryMock);

            controller.Create(new Anagram());

            Assert.Greater(controller.ModelState.ErrorCount, 0);
        }
    }
}
