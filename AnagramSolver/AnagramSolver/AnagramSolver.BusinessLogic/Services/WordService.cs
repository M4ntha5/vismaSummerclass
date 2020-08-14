using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly IAdditionalWordRepository _additionalWordRepository;
        private readonly IMapper _mapper;

        public WordService(IWordRepository wordRepository, IAdditionalWordRepository additionalWordRepo, IMapper mapper)
        {
            _wordRepository = wordRepository;
            _additionalWordRepository = additionalWordRepo;
            _mapper = mapper;
        }

        public async Task<List<Anagram>> GetAllWords()
        {
            var resultEntity = await _wordRepository.GetAllWords();
            var words = _mapper.Map<List<Anagram>>(resultEntity);

            return words;
        }

        public async Task<List<Anagram>> GetWordsBySearch(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                throw new Exception("Cannot find any words, because your phrase is empty");

            var resultEntity = await _additionalWordRepository.SelectWordsBySearch(phrase);
            var words = _mapper.Map<List<Anagram>>(resultEntity);
            return words;
        }

        public async Task InsertWord(Anagram anagram)
        {
            if (anagram == null || string.IsNullOrEmpty(anagram.Category) ||
                string.IsNullOrEmpty(anagram.Word))
                throw new Exception("Canno add Word, because Word is empty");

            var sortedWord = String.Concat(anagram.Word.OrderBy(x => x));
            var existingAnagrams = await _wordRepository.GetSelectedWordAnagrams(sortedWord);

            ChechForDuplicates(existingAnagrams, anagram);

            await _wordRepository.AddNewWord(anagram);
        }

        public async Task<List<Anagram>> GetWordAnagrams(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new Exception("Cannot find any anagrams, because word is not defined");

            var results = await _wordRepository.GetSelectedWordAnagrams(word);
            var anagrams = _mapper.Map<List<Anagram>>(results);

            return anagrams;
        }

        private void ChechForDuplicates(List<WordEntity> existingAnagrams, Anagram newAnagram)
        {
            if (existingAnagrams.Count > 0)
                foreach (var item in existingAnagrams)
                    if (item.Word == newAnagram.Word)
                        throw new Exception($"Word {newAnagram.Word} already exists");
        }

        public async Task<Anagram> GetWordById(int? id)
        {
            if (id == null)
                throw new Exception("Id not defined");

            var wordEntity = await _additionalWordRepository.SelectWordById((int)id);
            if (wordEntity == null)
                throw new Exception("No word with sepcified Id");

            var model = _mapper.Map<Anagram>(wordEntity);

            return model;
        }

        public Task DeleteWordById(int id)
        {
            if (id < 1)
                throw new Exception("Word with provided Id do not exist");

            return _additionalWordRepository.DeleteSelectedWord(id);
        }

        public Task UpdateWord(int id, Anagram newWord)
        {
            if (newWord == null || string.IsNullOrEmpty(newWord.Word) || 
                string.IsNullOrEmpty(newWord.Category) || id < 1)
                throw new Exception("Cannot update Word, because Word is empty");

            return _additionalWordRepository.UpdateSelectedWord(id, newWord);         
        }
    }
}
