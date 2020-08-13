using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class WordRepositoryEF : IWordRepository, IAdditionalWordRepository
    {
        ////context for DB First approach
        //private readonly AnagramSolverDBFirstContext _context;
        //public WordRepositoryEF(AnagramSolverDBFirstContext context)
        //{
        //    _context = context;
        //}

        //context for Code First approach
        private readonly AnagramSolverCodeFirstContext _context;
        public WordRepositoryEF(AnagramSolverCodeFirstContext context)
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

            await _context.Words.AddAsync(entity);
        }

        public Task<List<WordEntity>> GetAllWords()
        {
            return _context.Words.ToListAsync();
        }

        public Task<List<WordEntity>> GetSelectedWordAnagrams(string word)
        {
            var sortedword = String.Concat(word.OrderBy(x => x));
            return _context.Words.Where(x => x.SortedWord == sortedword).ToListAsync();
        }

        public ValueTask<WordEntity> SelectWordById(int id)
        {
            var wordEntity = _context.Words.FindAsync(id);
            if (wordEntity == null)
                throw new Exception("Word with provided Id not found");

            return wordEntity;
        }

        public Task<List<WordEntity>> SelectWordsBySearch(string phrase)
        {
            var wordsFound = _context.Words.Where(x => x.Word.Contains(phrase.ToLower())).ToListAsync();
            return wordsFound;
        }

        public async Task UpdateSelectedWord(int id, Anagram updatedWord)
        {
            var entity = await _context.Words.FindAsync(id);
            if (entity == null)
                throw new Exception("Word you are trying to update does not exist");

            entity.Category = updatedWord.Case;
            entity.Word = updatedWord.Word;
            entity.SortedWord = String.Concat(updatedWord.Word.OrderBy(x => x));               
        }

        public async Task DeleteSelectedWord(int id)
        {
            var entity = await _context.Words.FindAsync(id);
            _context.Words.Remove(entity);
        }
    }
}
