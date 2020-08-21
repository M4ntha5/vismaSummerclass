using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
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
    public class SearchHistoryControllerTests
    {
        IUserLogService _userLogServiceMock;
        ISearchHistoryService _searchHistoryServiceMock;

        SearchHistoryController _controller;

        [SetUp]
        public void Setup()
        {
            _userLogServiceMock = Substitute.For<IUserLogService>();
            _searchHistoryServiceMock = Substitute.For<ISearchHistoryService>();

            _controller = new SearchHistoryController(
                _userLogServiceMock, _searchHistoryServiceMock);
        }

        [Test]
        public async Task IndexSuccessWhenLogsFound()
        {
            var logs = new List<UserLog>()
            {
                new UserLog("123", "phrase", TimeSpan.FromSeconds(4), "action")
            };
            var returnList = new List<string>() { "word1", "word2" };

            _userLogServiceMock.GetAllSolverLogs().Returns(logs);
            _searchHistoryServiceMock.GetSearchedAnagrams(Arg.Any<string>()).Returns(returnList);

            var result = await _controller.Index() as ViewResult;
            var data = result.ViewData.Model as List<SearchHistory>;

            await _userLogServiceMock.Received().GetAllSolverLogs();
            await _searchHistoryServiceMock.Received().GetSearchedAnagrams(Arg.Any<string>());
            Assert.AreEqual(logs.Count, data.Count);
            Assert.AreEqual(logs[0].Ip, data[0].Ip);
            Assert.AreEqual(returnList.Count, data[0].Anagrams.Count);
            Assert.AreEqual(returnList[0], data[0].Anagrams[0]);
        }
        [Test]
        public async Task IndexSuccessWhenNoLogsFound()
        {
            _userLogServiceMock.GetAllSolverLogs().Returns(new List<UserLog>());

            var result = await _controller.Index() as ViewResult;
            var data = result.ViewData.Model as List<SearchHistory>;

            await _userLogServiceMock.Received().GetAllSolverLogs();

            Assert.IsEmpty(data);
        }

    }
}
