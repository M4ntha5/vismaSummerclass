using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Models
{
    public class AnagramsWithIds
    {
        public List<string> Anagrams { get; set; }
        public List<int> Ids { get; set; }

    }
}
