using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.EF.DatabaseFirst.Repositories
{
    public class UserLogRepositoryEF : IUserLogRepository
    {
        private readonly AnagramSolverContext _context;

        public UserLogRepositoryEF(AnagramSolverContext context)
        {
            _context = context;
        }

        public async Task InsertLog(UserLog userLog)
        {
            if (string.IsNullOrEmpty(userLog.Ip) || string.IsNullOrEmpty(userLog.SearchPhrase) ||
                TimeSpan.Zero == userLog.SearchTime)
                throw new Exception("Cannot add UserLog, because UserLog is empty");

            var entity = new UserLogEntity
            {
                Ip = userLog.Ip,
                SearchTime = userLog.SearchTime,
                Phrase = userLog.SearchPhrase
            };

            _context.UserLogs.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserLogEntity>> GetAllLogs()
        {
            return await _context.UserLogs.ToListAsync();
        }
    }
}
