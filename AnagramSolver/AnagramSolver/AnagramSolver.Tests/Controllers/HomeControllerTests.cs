﻿using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.WebApp.Controllers;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
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
    public class HomeControllerTests
    {
        IAnagramSolver _anagramSolverMock;
        ICookiesHandlerService _cookiesHandlerServiceMock;
        IUserLogService _userLogServiceMock;
        ISearchHistoryService _searchHistoryServiceMock;
        AnagramSolverCodeFirstContext _contextMock;

        HomeController _controller;

        [SetUp]
        public void Setup()
        {
            _anagramSolverMock = Substitute.For<IAnagramSolver>();
            _cookiesHandlerServiceMock = Substitute.For<ICookiesHandlerService>();
            _userLogServiceMock = Substitute.For<IUserLogService>();
            _searchHistoryServiceMock = Substitute.For<ISearchHistoryService>();
            _contextMock = Substitute.For<AnagramSolverCodeFirstContext>();

            _controller = new HomeController(_anagramSolverMock, _cookiesHandlerServiceMock,
                _userLogServiceMock, _searchHistoryServiceMock, _contextMock);
        }

        [Test]
        public async Task IndexFailedWhenInputEmpty()
        {
            var result = await _controller.Index("labas") as ViewResult;
            var data = result.ViewData.Model as List<string>;

            Assert.IsNull(data);
        }

        [Test]
        public async Task IndexFailedWhenNoSolvesLeftForIp()
        {
            _userLogServiceMock.CountAnagramsLeftForIpToSolve().Returns(0);

            var result = await _controller.Index("labas") as ViewResult;
            var data = result.ViewData.Values;

            await _userLogServiceMock.Received().CountAnagramsLeftForIpToSolve();
            Assert.AreEqual(1, data.Count);
        }

        [Test]
        public async Task IndexSuccessWhenResultsFoundInCachedWord()
        {
            var anagramsList = new List<string>() { "word1", "word2" };

            _userLogServiceMock.CountAnagramsLeftForIpToSolve().Returns(5);
            _searchHistoryServiceMock.GetSearchedAnagrams(Arg.Any<string>()).Returns(anagramsList);

            var result = await _controller.Index("labas") as ViewResult;
            var data = result.ViewData.Model as List<string>;

            await _userLogServiceMock.Received().CountAnagramsLeftForIpToSolve();
            await _searchHistoryServiceMock.Received().GetSearchedAnagrams(Arg.Any<string>());
            Assert.AreEqual(anagramsList.Count, data.Count);
            Assert.AreEqual(anagramsList[0], data[0]);
            Assert.AreEqual(anagramsList[1], data[1]);
        }

        [Test]
        public async Task IndexSuccessWhenSearchingDBForResult()
        {
            var returnList = new List<string>() { "word1", "word2" };

            _userLogServiceMock.CountAnagramsLeftForIpToSolve().Returns(5);
            _searchHistoryServiceMock.GetSearchedAnagrams(Arg.Any<string>()).Returns((List<string>)null);
            _anagramSolverMock.GetAnagrams(Arg.Any<string>()).Returns(returnList);
            await _userLogServiceMock.AddLog(Arg.Any<TimeSpan>(), Arg.Any<UserActionTypes>(), Arg.Any<string>());
            _cookiesHandlerServiceMock.AddCookie(Arg.Any<string>(), Arg.Any<string>());
            await _contextMock.SaveChangesAsync();

            var result = await _controller.Index("labas") as ViewResult;
            var data = result.ViewData.Model as List<string>;

            await _userLogServiceMock.Received().CountAnagramsLeftForIpToSolve();
            await _searchHistoryServiceMock.Received().GetSearchedAnagrams(Arg.Any<string>());
            await _anagramSolverMock.Received().GetAnagrams(Arg.Any<string>());
            await _userLogServiceMock.Received().AddLog(Arg.Any<TimeSpan>(), Arg.Any<UserActionTypes>(), Arg.Any<string>());
            _cookiesHandlerServiceMock.Received().AddCookie(Arg.Any<string>(), Arg.Any<string>());
            await _contextMock.Received().SaveChangesAsync();

            Assert.AreEqual(returnList.Count, data.Count);
            Assert.AreEqual(returnList[0], data[0]);
            Assert.AreEqual(returnList[1], data[1]);
        }

        [Test]
        public void DisplayCookiesSuccessWhenSomeCookiesFound()
        {
            var resultDictionary = new Dictionary<string, string>()
            {
                {"key", "val" }
            };
            _cookiesHandlerServiceMock.GetCurrentCookies().Returns(resultDictionary);

            var result = _controller.DisplayCookies() as ViewResult;
            var data = result.ViewData.Model as Dictionary<string, string>;

            _cookiesHandlerServiceMock.Received().GetCurrentCookies();

            Assert.AreEqual(resultDictionary.Count, data.Count);
        }

        [Test]
        public void DisplayCookiesReturnsEmptyViewWhenNoCookiesFound()
        {
            _cookiesHandlerServiceMock.GetCurrentCookies().Returns(new Dictionary<string, string>());

            var result = _controller.DisplayCookies() as ViewResult;
            var data = result.ViewData.Model as Dictionary<string, string>;

            _cookiesHandlerServiceMock.Received().GetCurrentCookies();

            Assert.IsNull(data);
        }
    }
}
