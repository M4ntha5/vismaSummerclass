using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
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

        public WordService(IWordRepository wordRepository, IAdditionalWordRepository additionalWordRepo)
        {
            _wordRepository = wordRepository;
            _additionalWordRepository = additionalWordRepo;
        }

        public async Task<List<Anagram>> GetAllWords()
        {
            var resultEntity = await _wordRepository.GetAllWords();
            var words = MatchWordEntityToModel(resultEntity);

            return words;
        }

        public async Task<List<Anagram>> GetWordsBySearch(string phrase)
        {
            var resultEntity = await _additionalWordRepository.SelectWordsBySearch(phrase);
            var words = MatchWordEntityToModel(resultEntity);
            return words;
        }

        public async Task InsertWord(Anagram anagram)
        {
            if (anagram == null || string.IsNullOrEmpty(anagram.Case) ||
                string.IsNullOrEmpty(anagram.Word))
                throw new Exception("Canno add Word, because Word is empty");

            var sortedWord = String.Concat(anagram.Word.OrderBy(x => x));
            var existingAnagrams = await _wordRepository.GetSelectedWordAnagrams(sortedWord);

            ChechForDuplicates(existingAnagrams, anagram);

            await _wordRepository.AddNewWord(anagram);
        }

        public async Task<List<Anagram>> GetWordAnagrams(string word)
        {
            var results = await _wordRepository.GetSelectedWordAnagrams(word);
            var anagrams = MatchWordEntityToModel(results);

            return anagrams;
        }

        private void ChechForDuplicates(List<WordEntity> existingAnagrams, Anagram newAnagram)
        {
            if (existingAnagrams.Count > 0)
                foreach (var item in existingAnagrams)
                    if (item.Word == newAnagram.Word)
                        throw new Exception($"Word {newAnagram.Word} already exists");
        }

       
        private List<Anagram> MatchWordEntityToModel(List<WordEntity> entities)
        {
            List<Anagram> model = new List<Anagram>();
            foreach (var entity in entities)
                model.Add(new Anagram()
                {
                    Case = entity.Category,
                    Word = entity.Word
                });
            return model;
        }

        public async Task<string> GetWordById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("Id not defined");

            var word = await _additionalWordRepository.SelectWordById(id);

            if(string.IsNullOrEmpty(word))
                throw new Exception("No word with sepcified Id");

            return word;
        }
    }

}
