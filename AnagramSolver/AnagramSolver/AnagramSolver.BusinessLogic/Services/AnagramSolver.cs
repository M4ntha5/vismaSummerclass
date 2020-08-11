using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
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
        private readonly ICachedWordRepository _cachedWordRepository;
        private readonly IUserInterface _userInterface;

        public AnagramSolver(IWordRepository wordRepository, IUserInterface userInterface,
            ICachedWordRepository cachedWordRepository, IAdditionalWordRepository additionalWordRepository)
        {
            _wordRepository = wordRepository;
            _cachedWordRepository = cachedWordRepository;
            _userInterface = userInterface;
        }

        public async Task<IList<string>> GetAnagrams(string inputWords)
        {
            var joinedInput = _userInterface.ValidateInputData(inputWords);
            if (string.IsNullOrEmpty(joinedInput))
                throw new Exception("You must enter at least one word");

            //getting all dictionary
            var allWords = _wordRepository.GetAllWords();
            //sorting user phrase
            var sortedInput = String.Concat(joinedInput.OrderBy(x => x));

            var sortedList = new List<WordEntity>();
            sortedList = SortWordsContainingInInput(allWords, sortedInput);

            var tmpInput = sortedInput;

            //look for single word anagrams first
            var singleWord = GetAllSingleWordAnagrams(sortedInput);
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
            //all anagrams found for search phrase       
            var idsString = string.Join(";", idsList.Take(Settings.AnagramsToGenerate));

            //adding search to cached table
            await _cachedWordRepository.InsertCachedWord(new CachedWord(inputWords, idsString));

            return resultAnagrams.Take(Settings.AnagramsToGenerate).ToList();
        }

        private WordEntity ContainsKey(List<WordEntity> list, string word)
        {
            var foundEntity = list.Where(x => x.SortedWord == word).FirstOrDefault();
            if (foundEntity != null)
                return foundEntity;
            else
                return null;
        }     

        private List<WordEntity> GetAllSingleWordAnagrams(string word)
        {
            var anagrams = _wordRepository.GetSelectedWordAnagrams(word);

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
            return new string(charList);
        }

    }
}
