using AnagramSolver.BusinessLogicDB.Database;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramSolver.BusinessLogicDB.Services
{
    public class AnagramSolver : IAnagramSolver
    {
        private readonly IWordRepository FileRepository;
        private readonly CachedWordQueries _cachedWord;

        public AnagramSolver(IWordRepository fileRepo)
        {
            FileRepository = fileRepo;
            _cachedWord = new CachedWordQueries();
        }

        public IList<string> GetAnagrams(string inputWords)
        {
            var anagrams = FileRepository.GetSelectedWordAnagrams(inputWords);
            var result = anagrams.Select(x => x.Word).ToList();

            //inserting searched word to cached table
            var list = anagrams.Select(x => x.Id).ToList();

            var cachedWord = new CachedWord(inputWords, string.Join(";", list.ToArray()));
            _cachedWord.InsertCachedWord(cachedWord);

            return result;           
        }

    }
}
