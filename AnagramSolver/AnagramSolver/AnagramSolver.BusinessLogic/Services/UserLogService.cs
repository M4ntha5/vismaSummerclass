using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class UserLogService : IUserLogService
    {
        private readonly IUserLogRepository _userLogRepository;
        private readonly IMapper _mapper;

        public UserLogService(IUserLogRepository userLogRepository, IMapper mapper)
        {
            _userLogRepository = userLogRepository;
            _mapper = mapper;
        }

        public Task AddLog(TimeSpan timeElapsed, UserActionTypes actionType, string searchPhrase = null)
        {
            if (actionType == UserActionTypes.GetAnagrams && 
                (string.IsNullOrEmpty(searchPhrase) || TimeSpan.Zero == timeElapsed))
                throw new Exception("Cannot add UserLog, because UserLog is empty");
            else if (actionType != UserActionTypes.GetAnagrams && TimeSpan.Zero == timeElapsed)
                throw new Exception("Cannot add UserLog, because UserLog is empty");

            var log = new UserLog(GetUserIp(), searchPhrase, timeElapsed, actionType.ToString());

            return _userLogRepository.InsertLog(log);
        }

        public async Task<List<UserLog>> GetAllSolverLogs()
        {
            var logsEntities = await _userLogRepository.GetAllAnagramSolveLogs();
            if (logsEntities == null || logsEntities.Count < 1)
                return new List<UserLog>();

            var logs = _mapper.Map<List<UserLog>>(logsEntities);
            return logs;
        }

        public async Task<int> CountAnagramsLeftForIpToSolve()
        {
            var ip = GetUserIp();
            var timesSearched = await _userLogRepository.GetTimesIpMadeAction(ip, UserActionTypes.GetAnagrams);
            var timesNewWordAdded = await _userLogRepository.GetTimesIpMadeAction(ip, UserActionTypes.InsertWord);
            var timesWordDeleted = await _userLogRepository.GetTimesIpMadeAction(ip, UserActionTypes.DeleteWord);
            var timesWordUpdated = await _userLogRepository.GetTimesIpMadeAction(ip, UserActionTypes.UpdateWord);

            var timesLeftToSearch = Settings.MaxAnagramsForIp - timesSearched
                - timesWordDeleted + timesWordUpdated + timesNewWordAdded;

            return timesLeftToSearch;
        }

        private string GetUserIp()
        {
            var ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
            return ip;
        }
    }
}
