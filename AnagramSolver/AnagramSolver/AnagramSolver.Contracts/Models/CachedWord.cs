﻿namespace AnagramSolver.Contracts.Models
{
    public class CachedWord
    {
        public string SearchPhrase { get; set; }
        public string AnagramsIds { get; set; }

        public CachedWord(string searchPhrase, string anagrams)
        {
            SearchPhrase = searchPhrase;
            AnagramsIds = anagrams;
        }
        public CachedWord()
        {
        }
    }
}
