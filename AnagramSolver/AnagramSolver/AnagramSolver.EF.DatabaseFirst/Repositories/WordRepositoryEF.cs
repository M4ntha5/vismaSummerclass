using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.EF.DatabaseFirst.Repositories
{
    public class WordRepositoryEF : IWordRepository, IAdditionalWordRepository
    {
        private readonly AnagramSolverContext _context;

        public WordRepositoryEF(AnagramSolverContext context)
        {
            _context = context;
        }

        public async Task AddNewWord(Anagram anagram)
        {
            if (anagram == null || string.IsNullOrEmpty(anagram.Case) ||
                string.IsNullOrEmpty(anagram.Word))
                throw new Exception("Cannot add Word, because Word is empty");

            var sortedword = String.Concat(anagram.Word.OrderBy(x => x));

            var entity = new WordEntity
            {
                Category = anagram.Case,
                SortedWord = sortedword,
                Word = anagram.Word
            };

            _context.Words.Add(entity);
            await _context.SaveChangesAsync();
        }

        public Task ClearSelectedTable(List<string> tables)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WordEntity>> GetAllWords()
        {
            return await _context.Words.ToListAsync();
        }

        public async Task<List<WordEntity>> GetSelectedWordAnagrams(string word)
        {
            var sortedword = String.Concat(word.OrderBy(x => x));
            return await _context.Words.Where(x => x.SortedWord == sortedword).ToListAsync();
        }

        public async Task<string> SelectWordById(string id)
        {
            var parsedId = int.Parse(id);
            var wordEntity = await _context.Words.FindAsync(parsedId);
            var word = wordEntity.Word;
            if (string.IsNullOrEmpty(word))
                throw new Exception("Word with provided Id not found");
            return word;
        }

        public async Task<List<WordEntity>> SelectWordsBySearch(string phrase)
        {
            var wordsFound = await _context.Words.Where(x => x.Word.Contains(phrase.ToLower())).ToListAsync();
            return wordsFound;
        }




        /*
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

        public async Task<IEnumerable<Anagram>> GetWords()
        {
            var allWords = _context.Words;
            var words = new List<Anagram>();
            foreach (var word in allWords)
                words.Add(MatchWordEntityToModel(word));

            return words;
        }

        private Anagram MatchWordEntityToModel(WordEntity entity)
        {
            var model = new Anagram
            {
                Case = entity.Category,
                Word = entity.Word
            };
            return model;
        }*/
    }
}
