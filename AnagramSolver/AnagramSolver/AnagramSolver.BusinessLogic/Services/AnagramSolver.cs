using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class AnagramSolver : IAnagramSolver
    {
        private readonly IWordRepository _wordRepository;
        private readonly ICachedWordService _cachedWordService;

        public AnagramSolver(IWordRepository wordRepository, ICachedWordService cachedWordService)
        {
            _wordRepository = wordRepository;
            _cachedWordService = cachedWordService;
        }

        public async Task<IList<string>> GetAnagrams(string inputWords)
        {
            var joinedInput = ValidateInputData(inputWords);
            if (string.IsNullOrEmpty(joinedInput))
                throw new Exception("You must enter at least one word");

            //getting all dictionary
            var allWords = await _wordRepository.GetAllWords();
            if (allWords == null || allWords.Count < 1)
                throw new Exception("No anagrams found for your input");
            //sorting user phrase
            var sortedInput = String.Concat(joinedInput.OrderBy(x => x));

            var sortedList = new List<WordEntity>();
            sortedList = SortWordsContainingInInput(allWords, sortedInput);

            var tmpInput = sortedInput;

            //look for single word anagrams first
            var singleWord = await GetAllSingleWordAnagrams(sortedInput);
            if (singleWord == null)
                singleWord = new List<WordEntity>();

            var resultAnagrams = singleWord.Select(x => x.Word).ToList();
            var idsList = singleWord.Select(x => x.ID.ToString()).ToList();


            //look for multi word anagrams
            foreach (var elem in sortedList)
            {
                //assign element to current variable
                var current = elem;
                //trimming elem letters from input
                tmpInput = TrimLetters(tmpInput, current.SortedWord);
                foreach (var elem2 in sortedList)
                {
                    //both are then same - continue
                    if (current.SortedWord == elem2.SortedWord)
                        continue;

                    var foundWordsLength = elem.SortedWord.Length + tmpInput.Length;
                    //if leftover makes word and all words do not exceed input length
                    var contais = ContainsKey(sortedList, tmpInput);
                    if (contais != null && foundWordsLength == sortedInput.Length)
                    {
                        // adding found words to reult list
                        resultAnagrams.Add($"{elem.Word} {contais.Word}");
                        //adding found words to ids collection
                        idsList.Add($"{elem.ID}/{contais.ID}");
                        break;
                    }
                }
                //recover primary input
                tmpInput = sortedInput;
                if (resultAnagrams.Count == Settings.AnagramsToGenerate)
                    break;
            }

            //adding search to cached table
            await _cachedWordService.AddCachedWord(inputWords, idsList.Take(Settings.AnagramsToGenerate).ToList());

            return resultAnagrams.Take(Settings.AnagramsToGenerate).ToList();
        }

        private string ValidateInputData(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
                return null;
            var userWords = userInput.Split(' ').ToList();
            foreach (var word in userWords)
            {
                if (word.Length < Settings.MinInputLength)
                    throw new Exception($"Minimum one word length must be at least " +
                        $"{Settings.MinInputLength} characters!");
            }
            return string.Join("", userWords);
        }

        private WordEntity ContainsKey(List<WordEntity> list, string word)
        {
            var foundEntity = list.Where(x => x.SortedWord == word).FirstOrDefault();
            if (foundEntity != null)
                return foundEntity;
            else
                return null;
        }     

        private async Task<List<WordEntity>> GetAllSingleWordAnagrams(string word)
        {
            var anagrams = await _wordRepository.GetSelectedWordAnagrams(word);

            if (anagrams != null && anagrams.Count > 0)
                return anagrams;
            else
                return null;
        }

        private List<WordEntity> SortWordsContainingInInput(List<WordEntity> allWords, string sortedInput)
        {
            var sortedList = new List<WordEntity>();
            foreach (var word in allWords)
            {
                if (ContainsAll(sortedInput, word.Word))
                    sortedList.Add(word);
            }

            return sortedList;
        }

        private bool ContainsAll(string userInput, string word)
        {
            var input = userInput;
            for (int i = 0; i < word.Length; i++)
            {
                if (!input.Contains(word[i]))
                    return false;

                input = TrimLetters(input, word[i].ToString());
            }
            return true;
        }

        private string TrimLetters(string word, string letters)
        {
            var charList = word.ToCharArray();
            for (int i = 0; i < letters.Length; i++)
            {
                if (charList.Contains(letters[i]))
                {
                    string tmpWord = new string(charList);
                    var ind = tmpWord.IndexOf(letters[i]);
                    tmpWord = tmpWord.Remove(ind, 1);
                    charList = tmpWord.ToCharArray();
                }
            }
            return charList.ToString();
        }

    }
}
