using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Tests.IntegrationTests
{
    class AnagramsApiControllerTests
    {

        IUserInterface UserInterfaceMock;
        IAnagramSolver AnagramSolverMock;

        List<string> list;
        AnagramsAPIController Controller;

        [SetUp]
        public void Setup()
        {
            AnagramSolverMock = Substitute.For<IAnagramSolver>();
            UserInterfaceMock = Substitute.For<IUserInterface>();

            Controller = new AnagramsAPIController(UserInterfaceMock, AnagramSolverMock);

            list = new List<string>()
            {
                "bca", "cba", "acb", "bac", "cab", "abc"
            };
        }

        [Test]
        public void SolveAnagrams()
        {
            UserInterfaceMock.ValidateInputData(Arg.Any<string>()).Returns("abc");
            AnagramSolverMock.GetAnagrams(Arg.Any<string>()).Returns(list);

            var result = Controller.Get("abc") as ObjectResult;
            var data = result.Value as List<string>;

            UserInterfaceMock.Received().ValidateInputData(Arg.Any<string>());
            AnagramSolverMock.Received().GetAnagrams(Arg.Any<string>());
            Assert.AreEqual(list.Count, data.Count);
            Assert.AreEqual(list[0], data[0]);
        }

        [Test]
        public void SolveAnagramsWithBadInput()
        {
            var result = Controller.Get(null) as ObjectResult;

            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void SolveAnagramsDoNotPassValidations()
        {
            UserInterfaceMock.ValidateInputData(Arg.Any<string>()).Returns("");

            var result = Controller.Get("input") as ObjectResult;

            UserInterfaceMock.Received().ValidateInputData(Arg.Any<string>());
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}
