using AnagramSolver.Contracts.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AnagramSolver.Tests.IntegrationTests
{
    class HomeControllerTests
    {
        IUserInterface UserInterfaceMock;
        IAnagramSolver AnagramSolverMock;
        ICookiesHandler CookiesHandlerMock;
        IList<string> list;
        HomeController Controller;
        Dictionary<string, string> cookiesList;

        [SetUp]
        public void Setup()
        {
            UserInterfaceMock = Substitute.For<IUserInterface>();
            AnagramSolverMock = Substitute.For<IAnagramSolver>();
            CookiesHandlerMock = Substitute.For<ICookiesHandler>();

            Controller = new HomeController(UserInterfaceMock, AnagramSolverMock, CookiesHandlerMock);

            list = new List<string>()
            {
                "bca", "cba", "acb", "bac", "cab", "abc"
            };
            cookiesList = new Dictionary<string, string>()
            {
                {"key1", "val1" },
                {"key2", "val2" }
            };
        }

        [Test]
        public void SolveAnagrams()
        {
            UserInterfaceMock.ValidateInputData(Arg.Any<string>()).Returns("abc");
            AnagramSolverMock.GetAnagrams(Arg.Any<string>()).Returns(list);
            CookiesHandlerMock.GetCookieByKey(Arg.Any<string>());
            CookiesHandlerMock.AddCookie(Arg.Any<string>(), Arg.Any<string>());

            var result = Controller.Index("abc") as ViewResult;
            var data = result.ViewData.Model as List<string>;

            CookiesHandlerMock.Received().GetCookieByKey(Arg.Any<string>());
            UserInterfaceMock.Received().ValidateInputData(Arg.Any<string>());
            AnagramSolverMock.Received().GetAnagrams(Arg.Any<string>());
            Assert.AreEqual(list.Count, data.Count);
            Assert.AreEqual(list[0], data[0]);
        }

        [Test]
        public void SolveAnagramsGetDataFromCookies()
        {
            UserInterfaceMock.ValidateInputData(Arg.Any<string>()).Returns("abc");
            CookiesHandlerMock.GetCookieByKey(Arg.Any<string>()).Returns(string.Join(";", list.ToArray()));

            var result = Controller.Index("abc") as ViewResult;
            var data = result.ViewData.Model as List<string>;

            CookiesHandlerMock.Received().GetCookieByKey(Arg.Any<string>());
            UserInterfaceMock.Received().ValidateInputData(Arg.Any<string>());

            Assert.AreEqual(list.Count, data.Count);
            Assert.AreEqual(list[0], data[0]);
        }

        [Test]
        public void SolveAnagramsWithBadInput()
        {
            var result = Controller.Index(null) as ViewResult;

            Assert.AreEqual(1, result.ViewData.ModelState.ErrorCount);
        }

        [Test]
        public void SolveAnagramsDoNotPassValidations()
        {
            UserInterfaceMock.ValidateInputData(Arg.Any<string>()).Returns("");

            var result = Controller.Index("input") as ViewResult;

            UserInterfaceMock.Received().ValidateInputData(Arg.Any<string>());
            Assert.AreEqual(1, result.ViewData.ModelState.ErrorCount);
        }

        [Test]
        public void GetCurrentCookies()
        {
            CookiesHandlerMock.GetCurrentCookies().Returns(cookiesList);

            var result = Controller.DisplayCookies() as ViewResult;
            var data = result.ViewData.Model as Dictionary<string, string>;

            Assert.AreEqual(cookiesList.Count, data.Count);
            Assert.AreEqual(cookiesList.Keys.First(), data.Keys.First());
            Assert.AreEqual(cookiesList.Values.First(), data.Values.First());
        }
    }
}
