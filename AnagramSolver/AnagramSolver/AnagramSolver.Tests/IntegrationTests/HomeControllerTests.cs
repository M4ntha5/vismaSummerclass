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
        IList<string> list;


        [SetUp]
        public void Setup()
        {
            UserInterfaceMock = Substitute.For<IUserInterface>();
            AnagramSolverMock = Substitute.For<IAnagramSolver>();

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
            var controller = new HomeController(UserInterfaceMock, AnagramSolverMock);

            var result = controller.Index("abc");

            UserInterfaceMock.Received().ValidateInputData(Arg.Any<string>());
            AnagramSolverMock.Received().GetAnagrams(Arg.Any<string>());
            Assert.IsInstanceOf<ViewResult>(result);
        }
        [Test]
        public void TestHomeControllerIndexIdNotDefined()
        {
            var controller = new HomeController(UserInterfaceMock, AnagramSolverMock);

            controller.Index("");

            Assert.Greater(controller.ModelState.ErrorCount, 0);
        }
    }
}
