using AnagramSolver.BusinessLogic.Database;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class WordRepository : IWordRepository
    {       
        private readonly Dictionary<string, List<Anagram>> AllData;
        private readonly WordQueries _wordQueries;

        public WordRepository(WordQueries wordQueries)
        {
            AllData = new Dictionary<string, List<Anagram>>();
            _wordQueries = wordQueries;          
            ReadData();
        }

        private void ReadData()
        {
            var data = _wordQueries.SelectAllWords();
            foreach(var word in data)
            {
                if (AllData.ContainsKey(word.SortedWord))
                    AllData[word.SortedWord].Add(word);
                else
                    AllData.Add(word.SortedWord, new List<Anagram> { word });
            }
        }

        public void AddNewWord(Anagram anagram)
        {
            if (anagram == null || string.IsNullOrEmpty(anagram.Case) || 
                string.IsNullOrEmpty(anagram.Word))
                throw new Exception("You need to fill all the fields");

            anagram.SortedWord = String.Concat(anagram.Word.OrderBy(x => x));

            var existingAnagrams = _wordQueries.SelectWordAnagrams(anagram.SortedWord);
            
            ChechForDuplicates(existingAnagrams, anagram);

            _wordQueries.InsertWord(anagram);
        }

        private void ChechForDuplicates(List<Anagram> existingAnagrams, Anagram newAnagram)
        {
            if (existingAnagrams.Count > 0)
                foreach (var item in existingAnagrams)
                    if (item.Word == newAnagram.Word)
                        throw new Exception($"Word {newAnagram.Word} already exists");
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

        public Dictionary<string, List<Anagram>> GetAllData()
        {
            return AllData;
        }
    }
}
