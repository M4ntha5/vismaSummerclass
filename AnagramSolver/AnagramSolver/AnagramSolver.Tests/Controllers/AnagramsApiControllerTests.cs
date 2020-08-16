using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.Controllers
{
    [TestFixture]
    public class AnagramsApiControllerTests
    {
        IAnagramSolver _anagramSolverMock;
        AnagramSolverCodeFirstContext _contexMock;

        List<string> _stringList;
        AnagramsAPIController _controller;

        [SetUp]
        public void Setup()
        {
            _anagramSolverMock = Substitute.For<IAnagramSolver>();
            _contexMock = Substitute.For<AnagramSolverCodeFirstContext>();

            _controller = new AnagramsAPIController(_anagramSolverMock, _contexMock);

            _stringList = new List<string>() { "bca", "cba" };
        }

        [Test]
        public async Task GetSuccessWhenWordDefiled()
        {
            _anagramSolverMock.GetAnagrams(Arg.Any<string>()).Returns(_stringList);

            var result = await _controller.Get("abc") as ObjectResult;
            var data = result.Value as List<string>;

            await _anagramSolverMock.Received().GetAnagrams(Arg.Any<string>());
            Assert.AreEqual(_stringList.Count, data.Count);
            Assert.AreEqual(_stringList[0], data[0]);
        }

        [Test]
        public async Task GetFailedWhenWordNotDefined()
        {
            var result = await _controller.Get(null) as ObjectResult;

            Assert.AreEqual(400, result.StatusCode);
        }
    }
}
