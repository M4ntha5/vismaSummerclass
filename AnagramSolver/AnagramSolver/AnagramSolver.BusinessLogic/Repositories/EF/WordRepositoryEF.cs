using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

            _context.Words.Add(entity);
            await _context.SaveChangesAsync();
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

        public async Task<WordEntity> SelectWordById(int id)
        {
            var wordEntity = await _context.Words.FindAsync(id);
            if (wordEntity == null)
                throw new Exception("Word with provided Id not found");

            return wordEntity;
        }

        public async Task<List<WordEntity>> SelectWordsBySearch(string phrase)
        {
            var wordsFound = await _context.Words.Where(x => x.Word.Contains(phrase.ToLower())).ToListAsync();
            return wordsFound;
        }

        public async Task UpdateSelectedWord(int id, Anagram updatedWord)
        {
            var entity = await _context.Words.FindAsync(id);
            if (entity == null)
                throw new Exception("Word you are trying to update does not exist");

            var newEntity = new WordEntity
            {
                Category = updatedWord.Case,
                SortedWord = String.Concat(updatedWord.Word.OrderBy(x => x)),
                Word = updatedWord.Word,
                ID = entity.ID
            };

            _context.Entry(entity).CurrentValues.SetValues(newEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSelectedWord(int id)
        {
            var entity = new WordEntity() { ID = id };

            _context.Words.Attach(entity);
            _context.Words.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
