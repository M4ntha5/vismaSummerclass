using AnagramSolver.Contracts.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using AnagramSolver.WebApp.Controllers;
using AnagramSolver.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.Tests.IntegrationTests
{
    class HomeControllerTests
    {
        IUserInterface UserInterfaceMock;
        IAnagramSolver AnagramSolverMock;
        ICookiesHandler CookiesHandlerMock;
        IList<string> list;
        HomeController Controller;

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
        }

        [Test]
        public void TestHomeControllerIndex()
        {
            UserInterfaceMock.ValidateInputData(Arg.Any<string>()).Returns("abc");
            AnagramSolverMock.GetAnagrams(Arg.Any<string>()).Returns(list);
            CookiesHandlerMock.GetCookieByKey(Arg.Any<string>());

            var result = Controller.Index("abc");

            CookiesHandlerMock.Received().GetCookieByKey(Arg.Any<string>());
            UserInterfaceMock.Received().ValidateInputData(Arg.Any<string>());
            AnagramSolverMock.Received().GetAnagrams(Arg.Any<string>());
            Assert.IsInstanceOf<ViewResult>(result);
        }
        [Test]
        public void TestHomeControllerIndexIdNotDefined()
        {
            Controller.Index("");

            Assert.Greater(Controller.ModelState.ErrorCount, 0);
        }
    }
}
