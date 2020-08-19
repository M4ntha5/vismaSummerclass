using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Enums;
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
            var entity = new UserLogEntity
            {
                Ip = userLog.Ip,
                SearchTime = userLog.SearchTime,
                Phrase = userLog.SearchPhrase,
                Action = userLog.Action
            };

            await _context.UserLogs.AddAsync(entity);
        }

        public Task<List<UserLogEntity>> GetAllAnagramSolveLogs()
        {
            return _context.UserLogs.Where(
                x => x.Action == UserActionTypes.GetAnagrams.ToString()).ToListAsync();
        }

        public Task<int> GetTimesIpMadeAction(string ip, UserActionTypes action)
        {
            return _context.UserLogs.CountAsync(x => x.Ip == ip && x.Action == action.ToString());
        }
    }
}
