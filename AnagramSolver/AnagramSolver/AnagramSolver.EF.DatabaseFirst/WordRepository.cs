using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.DatabaseFirst.Data;
using AnagramSolver.EF.DatabaseFirst.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.EF.DatabaseFirst
{
    public class WordRepository : IWordRepository
    {
        private readonly AnagramSolverWebAppContext _context;
        private readonly Dictionary<string, List<Anagram>> AllData;

        public WordRepository(AnagramSolverWebAppContext context)
        {
            AllData = new Dictionary<string, List<Anagram>>();
            _context = context;
            ReadData();
        }

        private void ReadData()
        {
            var data = _context.Words.ToList();
            foreach (var word in data)
            {
                var anagramModel = MatchWordEntityToModel(word);

                if (AllData.ContainsKey(word.SortedWord))
                    AllData[word.SortedWord].Add(anagramModel);
                else
                    AllData.Add(word.SortedWord, new List<Anagram> { anagramModel });
            }
        }

        public void AddNewWord(Anagram anagram)
        {
            if (anagram == null || string.IsNullOrEmpty(anagram.Case) ||
                string.IsNullOrEmpty(anagram.Word))
                throw new Exception("You need to fill all the fields");

            anagram.SortedWord = String.Concat(anagram.Word.OrderBy(x => x));

            var existingAnagrams = GetSelectedWordAnagrams(anagram.SortedWord);

            ChechForDuplicates(existingAnagrams, anagram);

            var entity = new WordEntity
            {
                Category = anagram.Case,
                SortedWord = anagram.SortedWord,
                Word = anagram.Word
            };

            _context.Words.Add(entity);
            _context.SaveChanges();
        }
        private void ChechForDuplicates(List<Anagram> existingAnagrams, Anagram newAnagram)
        {
            if (existingAnagrams.Count > 0)
                foreach (var item in existingAnagrams)
                    if (item.Word == newAnagram.Word)
                        throw new Exception($"Word {newAnagram.Word} already exists");
        }

        public Dictionary<string, List<Anagram>> GetAllData()
        {
            return AllData;
        }

        public List<Anagram> GetSelectedWordAnagrams(string key)
        {
            var anagrams = _context.Words.Where(x=> x.SortedWord == key);
            var anagramsModel = new List<Anagram>();
            foreach (var anagram in anagrams)
                anagramsModel.Add(MatchWordEntityToModel(anagram));

            return anagramsModel;
        }

        public List<Anagram> GetWords()
        {
            var allWords = _context.Words.ToList();
            var words = new List<Anagram>();
            foreach (var word in allWords)
                words.Add(MatchWordEntityToModel(word));

            return words;
        }

        private Anagram MatchWordEntityToModel(WordEntity entity)
        {
            var model = new Anagram
            {
                Id = entity.ID,
                Case = entity.Category,
                SortedWord = entity.SortedWord,
                Word = entity.Word
            };
            return model;
        }
    }
}
