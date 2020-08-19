using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class CachedWordService : ICachedWordService
    {
        private readonly ICachedWordRepository _cachedWordRepository;
        private readonly IMapper _mapper;

        public CachedWordService(ICachedWordRepository cachedWordRepository, IMapper mapper)
        {
            _cachedWordRepository = cachedWordRepository;
            _mapper = mapper;
        }

        public async Task<CachedWord> GetSelectedCachedWord(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                throw new Exception("Cannont find CachedWord, because phrase is empty");

            var cachedWordEntity = await _cachedWordRepository.GetCachedWord(phrase);
            if (cachedWordEntity == null)
                return null;
            var cachedWord = _mapper.Map<CachedWord>(cachedWordEntity);

            return cachedWord;
        }

        public Task AddCachedWord(string phrase, List<string> anagramsIds)
        {
            if (string.IsNullOrEmpty(phrase) || anagramsIds == null || anagramsIds.Count < 1)
                throw new Exception("Cannot add CachedWord, because CachedWord is empty");

            var formatedIds = string.Join(";", anagramsIds);
            var cachedWord = new CachedWord(phrase, formatedIds);

            return _cachedWordRepository.InsertCachedWord(cachedWord);
        }

    }
}
