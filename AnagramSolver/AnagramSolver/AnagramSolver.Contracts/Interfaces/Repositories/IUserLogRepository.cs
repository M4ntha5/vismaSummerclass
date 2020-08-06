using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IUserLogRepository
    {
        Task InsertLog(UserLog userLog);
        Task<List<UserLogEntity>> GetAllLogs();
    }
}
