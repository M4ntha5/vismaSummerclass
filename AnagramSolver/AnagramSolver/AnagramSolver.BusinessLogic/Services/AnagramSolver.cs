using AnagramSolver.BusinessLogic.Database;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.Contracts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramSolver.BusinessLogic.Services
{
    public class AnagramSolver : IAnagramSolver
    {
        private readonly IWordRepository FileRepository;
        private readonly CachedWordQueries _cachedWord;

        public AnagramSolver(IWordRepository fileRepo, CachedWordQueries cachedWordQueries)
        {
            FileRepository = fileRepo;
            _cachedWord = cachedWordQueries;
        }

        public IList<string> GetAnagrams(string inputWords)
        {
            //getting all dictionary
            var allWords = FileRepository.GetAllData();
            //sorting user phrase
            var sortedInput = String.Concat(inputWords.OrderBy(x => x));

            var sortedList = new Dictionary<string, List<Anagram>>();
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
            var singleWordResultTuple = GetAllSingleWordAnagrams(allWords, sortedInput);

            var singleWordResult = singleWordResultTuple.Item1;
            var singleWordResultIds = singleWordResultTuple.Item2;

            if (singleWordResult == null)
                singleWordResult = new List<string>();


            var multiWordResult = new Tuple<List<string>, List<int>>(new List<string>(), new List<int>());
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
            //all words ids for this found anagrams
            singleWordResultIds.AddRange(multiWordResult.Item2);
            //all anagrams found for search phrase
            singleWordResult.AddRange(multiWordResult.Item1);

            var idsString = string.Join(";", singleWordResultIds.Take(Settings.AnagramsToGenerate));
            //adding search to cached table
            _cachedWord.InsertCachedWord(new CachedWord(inputWords, idsString));

            
            return singleWordResult.Take(Settings.AnagramsToGenerate).ToList();
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

            foreach(var el in sortedData)
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
            foreach(var val in list)
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

        private Tuple<List<string>, List<int>> GetAllSingleWordAnagrams(
            Dictionary<string, List<Anagram>> allWords, string sortedInput)
        {
            if (allWords.ContainsKey(sortedInput))
            {
                Tuple<List<string>, List<int>> results = 
                    new Tuple<List<string>, List<int>>(new List<string>(), new List<int>());

                allWords[sortedInput].ForEach(
                    x => { 
                        results.Item1.Add(x.Word); 
                        results.Item2.Add(x.Id);
                    });

                //removes user input from results
                results.Item1.Remove(sortedInput);
                return results;
            }
            else
                return null;
        }

        private bool ContainsAll(string userInput, string word)
        {
            var input = userInput;
            for (int i=0;i< word.Length; i++)
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

        private Tuple<List<string>, List<int>> AddToResultList(List<Anagram> firstAngarams, List<Anagram> secondAngarams, Tuple<List<string>, List<int>> results)
        {
            foreach (var first in firstAngarams)
            {
                results.Item2.Add(first.Id);
                foreach (var second in secondAngarams)
                {
                    results.Item1.Add($"{first.Word} {second.Word}");
                    results.Item2.Add(second.Id);
                }
            }

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
