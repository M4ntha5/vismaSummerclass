using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramSolver.BusinessLogic.Services
{
    public class AnagramSolver : IAnagramSolver
    {
        public IWordRepository FileRepository { get; set; }

        public IList<string> GetAnagrams(string inputWords)
        {
            //getting all dictionary
            var allWords = FileRepository.ReadDataFromFile();
            //sorting user phrase
            var sortedInput = String.Concat(inputWords.OrderBy(x => x));

            var sortedList = new Dictionary<string, List<Anagram>>();
            sortedList = SortWordsContainingInInput(allWords, sortedInput);

            var tmpInput = sortedInput;
            var multiWordResult = new List<string>();

            //look for single word anagrams first
            var singleWordResult = GetAllSingleWordAnagrams(allWords, sortedInput);

            if (singleWordResult == null)
                singleWordResult = new List<string>();

            //look for multi word anagrams
            foreach (var elem in sortedList)
            {
                //assign element to current variable
                var current = elem;
                //trimming elem letters from input
                tmpInput = TrimLetters(tmpInput, current.Key);
                foreach (var elem2 in sortedList)
                {
                    //both are then same - continue
                    if (current.Key == elem2.Key)
                        continue;

                    var foundWordsLength = elem.Key.Length + tmpInput.Length;
                    //if leftover makes word and all words do not exceed input length
                    if (sortedList.ContainsKey(tmpInput) && foundWordsLength == sortedInput.Length)
                    {
                        // adding found words to reult list
                        multiWordResult = AddToResultList(elem.Value, sortedList[tmpInput], multiWordResult);
                        break;
                    }
                }
                //recover primary input
                tmpInput = sortedInput;
            }

            singleWordResult.AddRange(multiWordResult);
            return singleWordResult.Take(Settings.AnagramsToGenerate).ToList();
        }

        private List<string> GetAllSingleWordAnagrams(
            Dictionary<string, List<Anagram>> allWords, string sortedInput)
        {
            if (allWords.ContainsKey(sortedInput))
            {
                var results = new List<string>();
                allWords[sortedInput].ForEach(x => results.Add(x.Word));

                //removes user input from results
                results.Remove(sortedInput);
                return results;
            }
            else
                return null;
        }

        private bool ContainsAll(string userInput, string word)
        {
            for(int i=0;i< word.Length; i++)
            {
                if (!userInput.Contains(word[i]))
                    return false;
            }
            return true;
        }

        private string TrimLetters(string word, string letters)
        {
            var charList = word.ToCharArray();
            for(int i =0;i<letters.Length;i++)
            {
                if(charList.Contains(letters[i]))
                {
                    string tmpWord = new string(charList);
                    var ind = tmpWord.IndexOf(letters[i]);
                    tmpWord = tmpWord.Remove(ind, 1);
                    charList = tmpWord.ToCharArray();
                }
            }
            return new string(charList);
        }

        private List<string> AddToResultList(List<Anagram> firstAngarams, List<Anagram> secondAngarams, List<string> results)
        {
            foreach (var first in firstAngarams)
                foreach (var second in secondAngarams)
                    results.Add($"{first.Word} {second.Word}");

            return results;
        }

        private Dictionary<string, List<Anagram>> SortWordsContainingInInput(
            Dictionary<string, List<Anagram>> allWords, string sortedInput)
        {
            var sortedList = new Dictionary<string, List<Anagram>>();
            foreach (var word in allWords)
            {
                if (ContainsAll(sortedInput, word.Key))
                    sortedList.Add(word.Key, word.Value);
            }
            return sortedList;
        }
    }
}
