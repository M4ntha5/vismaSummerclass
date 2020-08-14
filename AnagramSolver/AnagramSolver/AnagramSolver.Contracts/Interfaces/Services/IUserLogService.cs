using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces.Services
{
    public interface IUserLogService
    {
        Task AddLog(TimeSpan timeElapsed, UserActionTypes actionType, string searchPhrase = null);
        Task<List<UserLog>> GetAllSolverLogs();
        Task<int> CountAnagramsLeftForIpToSolve();

    }
}
