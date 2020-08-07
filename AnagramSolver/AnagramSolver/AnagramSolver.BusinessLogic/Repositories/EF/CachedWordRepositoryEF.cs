using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.DatabaseFirst.Data;
using AnagramSolver.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.EF.CodeFirst;

namespace AnagramSolver.BusinessLogic.Repositories
{
    public class CachedWordRepositoryEF : ICachedWordRepository
    {
        ////context for DB First approach
        //private readonly AnagramSolverDBFirstContext _context;
        //public CachedWordRepositoryEF(AnagramSolverDBFirstContext context)
        //{
        //    _context = context;
        //}

        //context for Code First approach
        private readonly AnagramSolverCodeFirstContext _context;
        public CachedWordRepositoryEF(AnagramSolverCodeFirstContext context)
        {
            _context = context;
        }

        public async Task InsertCachedWord(CachedWord cachedWord)
        {
            if (string.IsNullOrEmpty(cachedWord.SearchPhrase) || string.IsNullOrEmpty(cachedWord.AnagramsIds))
                throw new Exception("Cannot add CachedWord, because CachedWord is empty");

            var entity = new CachedWordEntity
            {
                AnagramsIds = cachedWord.AnagramsIds,
                Phrase = cachedWord.SearchPhrase
            };

            _context.CachedWords.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<CachedWordEntity> GetCachedWord(string phrase)
        {
            var res = await _context.CachedWords.Where(x => x.Phrase == phrase).SingleOrDefaultAsync();
            
            return res;
        }
    }
}
