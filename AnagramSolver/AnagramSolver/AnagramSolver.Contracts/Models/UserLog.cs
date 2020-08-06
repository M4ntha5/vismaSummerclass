using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Models
{
    public class UserLog
    {
        public string Ip { get; set; }
        public string SearchPhrase { get; set; }
        public TimeSpan SearchTime { get; set; }

        public UserLog(string ip, string searchPhrase, TimeSpan searchTime)
        {
            Ip = ip;
            SearchPhrase = searchPhrase;
            SearchTime = searchTime;
        }
    }
}
