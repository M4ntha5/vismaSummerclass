using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.IntegrationTests.Repositories
{
    public class UserLogRepositoryEFTests
    {
        AnagramSolverCodeFirstContext _context;
        BusinessLogic.Repositories.UserLogRepositoryEF _repo;
        DbContextOptionsBuilder<AnagramSolverCodeFirstContext> options;
        [SetUp]
        public void SetUp()
        {
            options = new DbContextOptionsBuilder<AnagramSolverCodeFirstContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               ;

            _context = new AnagramSolverCodeFirstContext(options.Options);
            _repo = new BusinessLogic.Repositories.UserLogRepositoryEF(_context);
        }

        [Test]
        public async Task InsertLogSuccess()
        {
            var opt = new DbContextOptionsBuilder<AnagramSolverCodeFirstContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;
            using (var context = new AnagramSolverCodeFirstContext(opt))
            {
                var repo = new BusinessLogic.Repositories.UserLogRepositoryEF(context);
                var log = new UserLog(
                "169.35", "phrase15", TimeSpan.FromSeconds(5), UserActionTypes.GetAnagrams.ToString());

                await repo.InsertLog(log);
                await context.SaveChangesAsync();

                var all = await context.UserLogs.ToListAsync();
                var insertedLog = await context.UserLogs.Where(x => x.Phrase == log.SearchPhrase).SingleOrDefaultAsync();

                Assert.AreEqual(log.SearchPhrase, insertedLog.Phrase);
                Assert.AreEqual(log.Ip, insertedLog.Ip);
                Assert.AreEqual(log.Action, insertedLog.Action);
            }
        }

        [Test]
        public async Task GetAllAnagramSolveLogsSuccess()
        {
            var opt = new DbContextOptionsBuilder<AnagramSolverCodeFirstContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;
            using (var context = new AnagramSolverCodeFirstContext(opt))
            {
                var repo = new BusinessLogic.Repositories.UserLogRepositoryEF(context);
                var log = new UserLog("169.35", "phrase1", TimeSpan.FromSeconds(5), UserActionTypes.GetAnagrams.ToString());
                var log2 = new UserLog("169.359.54", "phrase2", TimeSpan.FromSeconds(6), UserActionTypes.GetAnagrams.ToString());

                await repo.InsertLog(log);
                await repo.InsertLog(log2);
                await context.SaveChangesAsync();

                var solveLogs = await repo.GetAllAnagramSolveLogs();

                Assert.AreEqual(2, solveLogs.Count);
                Assert.AreEqual(log.Ip, solveLogs[0].Ip);
                Assert.AreEqual(log.Action, solveLogs[0].Action);
                Assert.AreEqual(log2.Ip, solveLogs[1].Ip);
                Assert.AreEqual(log2.Action, solveLogs[1].Action);
            }
        }

        [Test]
        public async Task GetTimesIpMadeActionWhen2DeleteActionsMade()
        {
            var opt = new DbContextOptionsBuilder<AnagramSolverCodeFirstContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;
            using (var context = new AnagramSolverCodeFirstContext(opt))
            {
                var repo = new BusinessLogic.Repositories.UserLogRepositoryEF(context);
                var log = new UserLog("169.359.54", "phrase1", TimeSpan.FromSeconds(5), UserActionTypes.DeleteWord.ToString());
                var log2 = new UserLog("169.359.54", "phrase2", TimeSpan.FromSeconds(6), UserActionTypes.DeleteWord.ToString());

                await repo.InsertLog(log);
                await repo.InsertLog(log2);
                await context.SaveChangesAsync();

                var timesActionMade = await repo.GetTimesIpMadeAction(log.Ip, UserActionTypes.DeleteWord);

                Assert.AreEqual(2, timesActionMade);
            }
        }
    }
}
