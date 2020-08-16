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
        ICachedWordService _cachedWordServiceMock;
        IUserLogService _userLogServiceMock;
        IWordService _wordServiceMock;

        SearchHistoryController _controller;

        [SetUp]
        public void Setup()
        {
            _cachedWordServiceMock = Substitute.For<ICachedWordService>();
            _userLogServiceMock = Substitute.For<IUserLogService>();
            _wordServiceMock = Substitute.For<IWordService>();

            _controller = new SearchHistoryController(
                _wordServiceMock, _userLogServiceMock, _cachedWordServiceMock);
        }

        [Test]
        public async Task IndexSuccessWhenLogsFound()
        {
            var logs = new List<UserLog>()
            {
                new UserLog("123", "phrase", TimeSpan.FromSeconds(4), "action")
            };
            var words = new CachedWord("phrase", "1;2/8");
            var anagram = new Anagram() { Category = "cat1", Word = "word" };
            _userLogServiceMock.GetAllSolverLogs().Returns(logs);
            _cachedWordServiceMock.GetSelectedCachedWord(Arg.Any<string>()).Returns(words);
            _wordServiceMock.GetWordById(Arg.Any<int>()).Returns(anagram);

            var result = await _controller.Index() as ViewResult;
            var data = result.ViewData.Model as List<SearchHistory>;

            Assert.AreEqual(logs.Count, data.Count);
            Assert.AreEqual(logs[0].Ip, data[0].Ip);
        }
    }
}
