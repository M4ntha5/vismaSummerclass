using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using AnagramSolver.EF.CodeFirst;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class UserLogRepositoryEF : IUserLogRepository
    {
        ////context for DB First approach
        //private readonly AnagramSolverDBFirstContext _context;
        //public UserLogRepositoryEF(AnagramSolverDBFirstContext context)
        //{
        //    _context = context;
        //}

        //context for Code First approach
        private readonly AnagramSolverCodeFirstContext _context;
        public UserLogRepositoryEF(AnagramSolverCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertLog(UserLog userLog)
        {
            if (userLog.Action == UserActionTypes.GetAnagrams.ToString() && 
                (string.IsNullOrEmpty(userLog.Ip) || string.IsNullOrEmpty(userLog.SearchPhrase) ||
                TimeSpan.Zero == userLog.SearchTime))
                throw new Exception("Cannot add UserLog, because UserLog is empty");

            var entity = new UserLogEntity
            {
                Ip = userLog.Ip,
                SearchTime = userLog.SearchTime,
                Phrase = userLog.SearchPhrase,
                Action = userLog.Action
            };

            _context.UserLogs.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserLogEntity>> GetAllLogs()
        {
            return await _context.UserLogs.ToListAsync();
        }

        public async Task<int> GetAnagramsLeftForIpToSearch(string ip)
        {
            var timesSearched = await _context.UserLogs.CountAsync(
                x => x.Ip == ip && x.Action == UserActionTypes.GetAnagrams.ToString());

            var timesNewWordAdded = await _context.UserLogs.CountAsync(
                x => x.Ip == ip && x.Action == UserActionTypes.InsertWord.ToString());

            var timesWordDeleted = await _context.UserLogs.CountAsync(
                x => x.Ip == ip && x.Action == UserActionTypes.DeleteWord.ToString());

            var timesWordUpdated = await _context.UserLogs.CountAsync(
                x => x.Ip == ip && x.Action == UserActionTypes.UpdateWord.ToString());

            var timesLeftToSearch = Settings.MaxAnagramsForIp - timesSearched
                - timesWordDeleted + timesWordUpdated + timesNewWordAdded;

            return timesLeftToSearch;
        }
    }
}
