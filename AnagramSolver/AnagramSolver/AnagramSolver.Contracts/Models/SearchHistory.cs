using System;
using System.Collections.Generic;

namespace AnagramSolver.Contracts.Models
{
    public class SearchHistory
    {
        public string Ip { get; set; }
        public TimeSpan SearchTime { get; set; }
        public string SearchPhrase { get; set; }
        public List<string> Anagrams { get; set; }
    }
}
