using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.IntegrationTests.Repositories.SQL
{
    [TestFixture]
    public class UserLogRepositoryDBTests
    {
        UserLogRepositoryDB _repo;
        SqlConnection _sqlConnection;

        UserLog _log1, _log2;

        [SetUp]
        public void SetUp()
        {
            var conn = "Data Source=.;Initial Catalog=AnagramSolverTesting;Integrated Security=True";
            Settings.ConnectionStringDevelopment = conn;

            _sqlConnection = new SqlConnection()
            {
                ConnectionString = conn
            };

            _repo = new UserLogRepositoryDB();

            _log1 = new UserLog("169.359.54", "phrase1", TimeSpan.FromSeconds(5), 
                UserActionTypes.GetAnagrams.ToString());
            _log2 = new UserLog("169.359.54", "phrase2", TimeSpan.FromSeconds(6), 
                UserActionTypes.GetAnagrams.ToString());
        }
        [TearDown]
        public async Task TearDown()
        {
            //clear all table data
            TableHandler handler = new TableHandler();
            await handler.ClearSelectedTables(new List<string> { "UserLogs" });
        }

        private async Task<UserLogEntity> GetLogByPhrase(string phrase)
        {
            _sqlConnection.Open();
            SqlCommand cmd = new SqlCommand()
            {
                Connection = _sqlConnection,
                CommandType = CommandType.Text,
                CommandText = "select * from UserLogs where Phrase = @phrase"
            };
            cmd.Parameters.Add(new SqlParameter("@phrase", phrase));
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            var log = new UserLogEntity();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    log = 
                        new UserLogEntity()
                        {
                            ID = int.Parse(reader["ID"].ToString()),
                            Ip = reader["Ip"].ToString(),
                            Phrase = reader["Phrase"].ToString(),
                            SearchTime = TimeSpan.Parse(reader["SearchTime"].ToString()),
                            Action = reader["Action"].ToString()
                        };
                    break;
                }
            }
            return log;
        }

        [Test]
        public async Task InsertLogSuccess()
        {
            await _repo.InsertLog(_log1);

            var insertedLog = await GetLogByPhrase(_log1.SearchPhrase);

            Assert.AreEqual(_log1.SearchPhrase, insertedLog.Phrase);
            Assert.AreEqual(_log1.Ip, insertedLog.Ip);
            Assert.AreEqual(_log1.Action, insertedLog.Action);
            Assert.AreEqual(_log1.SearchTime, insertedLog.SearchTime);

        }

        [Test]
        public async Task GetAllAnagramSolveLogsSuccess()
        {        
            await _repo.InsertLog(_log1);
            await _repo.InsertLog(_log2);

            var solveLogs = await _repo.GetAllAnagramSolveLogs();

            Assert.AreEqual(2, solveLogs.Count);
            Assert.AreEqual(_log1.Ip, solveLogs[0].Ip);
            Assert.AreEqual(_log1.Action, solveLogs[0].Action);
            Assert.AreEqual(_log2.Ip, solveLogs[1].Ip);
            Assert.AreEqual(_log2.Action, solveLogs[1].Action);
        }

        [Test]
        public async Task GetTimesIpMadeActionWhen2DeleteActionsMade()
        {
            await _repo.InsertLog(_log1);
            await _repo.InsertLog(_log2);

            var timesActionMade = await _repo.GetTimesIpMadeAction(
                _log1.Ip, UserActionTypes.GetAnagrams);

            Assert.AreEqual(2, timesActionMade);
        }
    }
}
