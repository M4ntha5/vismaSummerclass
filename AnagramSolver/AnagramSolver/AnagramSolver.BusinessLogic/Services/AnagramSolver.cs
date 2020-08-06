﻿using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class AnagramSolver : IAnagramSolver
    {

        /* private readonly IWordRepository _wordRepository;
         private readonly ICachedWordRepository _cachedWordRepository;
         private readonly IUserInterface _userInterface;

         public AnagramSolver(IWordRepository wordRepository, IUserInterface userInterface,
             ICachedWordRepository cachedWordQueries)
         {
             _wordRepository = wordRepository;
             _cachedWordRepository = cachedWordQueries;
             _userInterface = userInterface;
         }

         public async Task<IList<string>> GetAnagrams(string inputWords)
         {

             return null;
         }
*/

        private readonly IWordRepository _wordRepository;
        private readonly ICachedWordRepository _cachedWordRepository;
        private readonly IUserInterface _userInterface;

        public AnagramSolver(IWordRepository wordRepository, IUserInterface userInterface,
            ICachedWordRepository cachedWordRepository)
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
            var allWords = await _wordRepository.GetAllWords();
            //sorting user phrase
            var sortedInput = String.Concat(joinedInput.OrderBy(x => x));

            var sortedList = new List<WordEntity>();
            sortedList = SortWordsContainingInInput(allWords, sortedInput);

            var tmpInput = sortedInput;


            /*  var res2 = SearchForWords(sortedInput, 0, sortedList, sortedInput.Length);

              var l = new List<List<int>>
              {
                  new List<int> { 1},
                  new List<int> { 2, 3},
                  new List<int> { 4, 5},
                  new List<int> { 6}
              };
              var anagramsList = new List<string>();
              foreach(var ang in res2)
              {
                  anagramsList.Add(Display(ang));
              }*/

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
                        resultAnagrams.Add($"{elem.Word} {contais.Word}");// = AddToResultList(elem.Value, sortedList[tmpInput], multiWordResult);
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


        public List<List<List<Anagram>>> SearchForWords(string phrase, int currLen,
            Dictionary<string, List<Anagram>> sortedData, int orgLen)
        {
            Dictionary<string, List<Anagram>> sorted2 =
                sortedData.ToDictionary(entry => entry.Key, entry => entry.Value);
            Dictionary<string, List<Anagram>> copy =
                sortedData.ToDictionary(entry => entry.Key, entry => entry.Value);

            string phrase2 = phrase;
            bool firstRemoved = false;
            KeyValuePair<string, List<Anagram>> firstFound = new KeyValuePair<string, List<Anagram>>();


            List<List<Anagram>> found = new List<List<Anagram>>();
            List<List<List<Anagram>>> foundTotal = new List<List<List<Anagram>>>();

            foreach (var el in sortedData)
            {
                foreach (var elem in sorted2)
                {
                    if (ContainsAll(phrase2, elem.Key))
                    {
                        if (!firstRemoved)
                        {
                            firstFound = elem;
                            copy.Remove(elem.Key);
                        }
                        currLen += elem.Key.Length;
                        sorted2.Remove(elem.Key);
                        phrase2 = TrimLetters(phrase2, elem.Key);
                        found.Add(elem.Value);
                        firstRemoved = true;
                    }
                    if (currLen == orgLen)
                    {
                        foundTotal.Add(found);
                        break;
                    }
                    if (currLen > orgLen)
                    {
                        found = new List<List<Anagram>>();
                        break;
                    }
                }
                currLen = 0;
                sorted2 = copy;
                phrase2 = phrase;
                firstRemoved = false;
                found = new List<List<Anagram>>();
                //removing first found elem and putting it to the end
                sorted2.Remove(firstFound.Key);
                sorted2.Add(firstFound.Key, firstFound.Value);
            }
            var ret = foundTotal;
            return ret;
        }

        string Display(List<List<Anagram>> list)
        {
            var myString = "";
            foreach (var val in list)
            {
                foreach (var anagram in val)
                {
                    myString += anagram.Word + "/";
                }
                //removing last /
                myString = myString.Remove(myString.Length - 1);
                //adding whitespace
                myString += " ";
            }
            return myString;

        }

        private async Task<List<WordEntity>> GetAllSingleWordAnagrams(string word)
        {
            var anagrams = await _wordRepository.GetSelectedWordAnagrams(word);

            if (anagrams != null && anagrams.Count > 0)
                return anagrams;
            else
                return null;


           /* if (allWords.ContainsKey(sortedInput))
            {
                var results = new Tuple<List<string>, List<string>>(new List<string>(), new List<string>());

                allWords[sortedInput].ForEach(
                    x =>
                    {
                        results.Item1.Add(x.Word);
                        results.Item2.Add(x.Id.ToString());
                    });

                //removes user input from results
                results.Item1.Remove(sortedInput);
                return results;
            }
            else
                return null;*/
        }

        private List<WordEntity> SortWordsContainingInInput(
          List<WordEntity> allWords, string sortedInput)
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

        private Tuple<List<string>, List<string>> AddToResultList(List<Anagram> firstAngarams, List<Anagram> secondAngarams, Tuple<List<string>, List<string>> results)
        {
            foreach (var first in firstAngarams)
            {
                foreach (var second in secondAngarams)
                {
                    results.Item1.Add($"{first.Word} {second.Word}");
                    results.Item2.Add($"{first.Id}/{second.Id}");
                }
            }

            return results;
        }


    }
}
