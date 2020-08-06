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
    public class CachedWordRepositoryEF : ICachedWordRepository
    {
        private readonly AnagramSolverContext _context;

        public CachedWordRepositoryEF(AnagramSolverContext context)
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
