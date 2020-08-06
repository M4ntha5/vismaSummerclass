using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    public class SearchHistoryController : Controller
    {
        private readonly ICachedWordRepository _cachedWordRepository;
        private readonly IUserLogRepository _userLogRepository;
        private readonly IWordService _wordService;

        public SearchHistoryController(IWordService wordService, IUserLogRepository userLogRepository,
            ICachedWordRepository cachedWordRepository)
        {
            _cachedWordRepository = cachedWordRepository;
            _userLogRepository = userLogRepository;
            _wordService = wordService;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _userLogRepository.GetAllLogs();

            List<SearchHistory> history = new List<SearchHistory>();
            foreach(var log in logs)
            {
                List<string> anagrams = new List<string>();
                var cached = await _cachedWordRepository.GetCachedWord(log.Phrase);
                if(cached != null)
                {
                    var anagramsIds = cached.AnagramsIds.Split(';').ToList();

                    foreach (var wordId in anagramsIds)
                    {
                        var phrase = wordId.Split('/').ToList();
                        string wordFound = "";
                        foreach (var word in phrase)
                        {
                            var found = await _wordService.GetWordById(word);
                            wordFound += found + " ";
                        }
                        anagrams.Add(wordFound);
                    }
                }
                history.Add(
                    new SearchHistory
                    {
                        Anagrams = anagrams,
                        Ip = log.Ip,
                        SearchPhrase = log.Phrase,
                        SearchTime = log.SearchTime
                    });
            }

            return View(history);
        }
    }
}
