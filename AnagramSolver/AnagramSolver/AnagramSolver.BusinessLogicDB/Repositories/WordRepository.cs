using AnagramSolver.BusinessLogicDB.Database;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.BusinessLogicDB.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly WordQueries _wordQueries;
        public WordRepository()
        {
            _wordQueries = new WordQueries();
        }

        public void AddWordToFile(Anagram anagram)
        {
            if (anagram == null || string.IsNullOrEmpty(anagram.Case) || 
                string.IsNullOrEmpty(anagram.Word))
                throw new Exception("You need to fill all the fields");

            anagram.SortedWord = String.Concat(anagram.Word.OrderBy(x => x));

            _wordQueries.InsertWord(anagram);
        }

        public List<Anagram> GetSelectedWordAnagrams(string key)
        {
            var anagrams = _wordQueries.SelectWordAnagrams(key);
            return anagrams;
        }

        public List<Anagram> GetWords()
        {
            var allWords = _wordQueries.SelectAllWords();
            return allWords;
        }

        public string GetDataFilePath()
        {
            throw new NotImplementedException();
        }
        public Dictionary<string, List<Anagram>> GetAllData()
        {
            throw new NotImplementedException();
        }
    }
}
