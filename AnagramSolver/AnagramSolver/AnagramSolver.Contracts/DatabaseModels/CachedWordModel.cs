using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.DatabaseModels
{
    public class CachedWordModel
    {       
        public string Id { get; set; }
        public string SearchPhrase { get; set; }
        public string Anagrams { get; set; }
    }
}
