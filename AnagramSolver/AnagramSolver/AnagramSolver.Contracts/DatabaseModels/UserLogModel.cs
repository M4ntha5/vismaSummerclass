using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.DatabaseModels
{
    public class UserLogModel
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string SearchPhrase { get; set; }
        public TimeSpan SearchTime { get; set; }
    }
}
