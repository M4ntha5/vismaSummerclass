using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Models
{
    public class CachedWord
    {       
        public string Id { get; set; }
        public string SearchPhrase { get; set; }
        public string AnagramsIds { get; set; }

        public CachedWord(string searchPhrase, string anagrams)
        {
            SearchPhrase = searchPhrase;
            AnagramsIds = anagrams;
        }
    }
}
