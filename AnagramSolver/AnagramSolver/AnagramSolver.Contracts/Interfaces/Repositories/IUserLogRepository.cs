using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IUserLogRepository
    {
        Task InsertLog(UserLog userLog);
        Task<List<UserLogEntity>> GetAllAnagramSolveLogs();
        Task<int> GetTimesIpMadeAction(string ip, UserActionTypes action);
    }
}
