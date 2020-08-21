using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class SearchHistoryService : ISearchHistoryService
    {
        private readonly ICachedWordService _cachedWordService;
        private readonly IWordService _wordService;

        public SearchHistoryService(ICachedWordService cachedWordService, IWordService wordService)
        {
            _cachedWordService = cachedWordService;
            _wordService = wordService;
        }

        public async Task<List<string>> GetSearchedAnagrams(string searchPhrase)
        {
            var anagrams = new List<string>();
            var cachedWord = await _cachedWordService.GetSelectedCachedWord(searchPhrase);
            if (cachedWord != null)
            {
                var anagramsIds = cachedWord.AnagramsIds.Split(';').ToList();

                foreach (var wordId in anagramsIds)
                {
                    var phrase = wordId.Split('/').ToList();
                    string wordFound = "";
                    foreach (var word in phrase)
                    {
                        var idToGet = int.Parse(word);
                        var anagram = await _wordService.GetWordById(idToGet);
                        wordFound += phrase.Count == 1 ? anagram.Word : anagram.Word + " ";
                    }
                    anagrams.Add(wordFound.Trim());
                }
            }
            return anagrams;
        }
    }
}
