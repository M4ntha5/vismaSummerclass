using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.Services
{
    [TestFixture]
    public class UserLogServiceTests
    {
        IUserLogRepository _logRepoMock;
        IMapper _mapperMock;
        IUserLogService _logService;

        [SetUp]
        public void Setup()
        {
            _logRepoMock = Substitute.For<IUserLogRepository>();
            _mapperMock = Substitute.For<IMapper>();

            _logService = new UserLogService(_logRepoMock, _mapperMock);
        }

        [Test]
        public void FailToAddLogIfActionIsNotGetAnagrams()
        {   
            Assert.ThrowsAsync<Exception>(
                async () => await _logService.AddLog(TimeSpan.Zero, UserActionTypes.DeleteWord));
        }

        [Test]
        public void FailToAddLogIfActionIsGetAnagrams()
        {
            Assert.ThrowsAsync<Exception>(
                async () => await _logService.AddLog(TimeSpan.FromSeconds(2), UserActionTypes.GetAnagrams, null));
        }

        [Test]
        public async Task SuccessToAddLogIfMandatoryFieldsAreFilled()
        {
            await _logRepoMock.InsertLog(Arg.Any<UserLog>());

            await _logService.AddLog(TimeSpan.FromMinutes(10), UserActionTypes.GetAnagrams, "oskaras");

            await _logRepoMock.Received().InsertLog(Arg.Any<UserLog>());
        }

        [Test]
        public async Task GetAllSolverLogsWhenListIsEmpty()
        {
            _logRepoMock.GetAllAnagramSolveLogs().Returns(new List<UserLogEntity>());
            _mapperMock.Map<List<UserLog>>(Arg.Any<List<UserLogEntity>>()).Returns(new List<UserLog>());

            var result = await _logService.GetAllSolverLogs();

            await _logRepoMock.Received().GetAllAnagramSolveLogs();
            _mapperMock.Received().Map<List<UserLog>>(Arg.Any<List<UserLogEntity>>());
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task GetAllSolverLogsWhenListIsNotEmpty()
        {
            var modelList = new List<UserLog>()
            {
                new UserLog("1233", "phrase", TimeSpan.FromSeconds(5), "action")
            };

            _logRepoMock.GetAllAnagramSolveLogs().Returns(new List<UserLogEntity>());
            _mapperMock.Map<List<UserLog>>(Arg.Any<List<UserLogEntity>>()).Returns(modelList);

            var result = await _logService.GetAllSolverLogs();

            await _logRepoMock.Received().GetAllAnagramSolveLogs();
            _mapperMock.Received().Map<List<UserLog>>(Arg.Any<List<UserLogEntity>>());
            Assert.AreEqual(modelList.Count, result.Count);
            Assert.AreEqual(modelList[0].Action, result[0].Action);
        }

        [Test]
        public async Task ZeroAnagramSolvesLeftForIp()
        {
            Settings.MaxAnagramsForIp = 2;
            _logRepoMock.GetTimesIpMadeAction(Arg.Any<string>(), Arg.Any<UserActionTypes>()).Returns(1, 0, 1, 0);

            var result = await _logService.CountAnagramsLeftForIpToSolve();

            await _logRepoMock.Received(4).GetTimesIpMadeAction(Arg.Any<string>(), Arg.Any<UserActionTypes>());
            Assert.AreEqual(0, result);
        }

        [Test]
        public async Task MoreThanZeroAnagramSolvesLeftForIp()
        {
            Settings.MaxAnagramsForIp = 2;
            _logRepoMock.GetTimesIpMadeAction(Arg.Any<string>(), Arg.Any<UserActionTypes>()).Returns(1, 5, 1, 0);

            var result = await _logService.CountAnagramsLeftForIpToSolve();

            await _logRepoMock.Received(4).GetTimesIpMadeAction(Arg.Any<string>(), Arg.Any<UserActionTypes>());
            Assert.AreEqual(5, result);
        }

    }
}
