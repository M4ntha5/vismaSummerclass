﻿using System;

namespace AnagramSolver.Contracts.Models
{
    public class UserLog
    {
        public string Ip { get; set; }
        public string SearchPhrase { get; set; }
        public TimeSpan SearchTime { get; set; }
        public string Action { get; set; }

        public UserLog(string ip, string searchPhrase, TimeSpan searchTime, string action)
        {
            Ip = ip;
            SearchPhrase = searchPhrase;
            SearchTime = searchTime;
            Action = action;
        }
    }
}
