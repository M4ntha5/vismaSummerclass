using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.IntegrationTests.Repositories.EF
{
    public class UserLogRepositoryEFTests
    {
        AnagramSolverCodeFirstContext _context;
        BusinessLogic.Repositories.UserLogRepositoryEF _repo;
        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<AnagramSolverCodeFirstContext>()
               .UseInMemoryDatabase(databaseName: "Test").Options;

            _context = new AnagramSolverCodeFirstContext(options);
            _repo = new BusinessLogic.Repositories.UserLogRepositoryEF(_context);
        }

        [Test]
        public async Task InsertLogSuccess()
        {
            var log = new UserLog(
            "169.35", "phrase15", TimeSpan.FromSeconds(5), UserActionTypes.GetAnagrams.ToString());

            await _repo.InsertLog(log);
            await _context.SaveChangesAsync();

            var all = await _context.UserLogs.ToListAsync();
            var insertedLog = await _context.UserLogs.Where(x => x.Phrase == log.SearchPhrase).SingleOrDefaultAsync();

            Assert.AreEqual(log.SearchPhrase, insertedLog.Phrase);
            Assert.AreEqual(log.Ip, insertedLog.Ip);
            Assert.AreEqual(log.Action, insertedLog.Action);
            
        }

        [Test]
        public async Task GetAllAnagramSolveLogsSuccess()
        {
            var log = new UserLog("169.35", "phrase1", TimeSpan.FromSeconds(5), UserActionTypes.GetAnagrams.ToString());
            var log2 = new UserLog("169.359.54", "phrase2", TimeSpan.FromSeconds(6), UserActionTypes.GetAnagrams.ToString());

            await _repo.InsertLog(log);
            await _repo.InsertLog(log2);
            await _context.SaveChangesAsync();

            var solveLogs = await _repo.GetAllAnagramSolveLogs();

            Assert.AreEqual(2, solveLogs.Count);
            Assert.AreEqual(log.Ip, solveLogs[0].Ip);
            Assert.AreEqual(log.Action, solveLogs[0].Action);
            Assert.AreEqual(log2.Ip, solveLogs[1].Ip);
            Assert.AreEqual(log2.Action, solveLogs[1].Action);
            
        }

        [Test]
        public async Task GetTimesIpMadeActionWhen2DeleteActionsMade()
        {
            var log = new UserLog("169.359.54", "phrase1", TimeSpan.FromSeconds(5), UserActionTypes.DeleteWord.ToString());
            var log2 = new UserLog("169.359.54", "phrase2", TimeSpan.FromSeconds(6), UserActionTypes.DeleteWord.ToString());

            await _repo.InsertLog(log);
            await _repo.InsertLog(log2);
            await _context.SaveChangesAsync();

            var timesActionMade = await _repo.GetTimesIpMadeAction(log.Ip, UserActionTypes.DeleteWord);

            Assert.AreEqual(2, timesActionMade);
            
        }
    }
}
