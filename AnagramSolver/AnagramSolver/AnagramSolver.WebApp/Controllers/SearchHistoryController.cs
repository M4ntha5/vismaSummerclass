using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.WebApp.Controllers
{
    public class SearchHistoryController : Controller
    {
        private readonly ICachedWordService _cachedWordService;
        private readonly IUserLogService _userLogService;
        private readonly IWordService _wordService;

        public SearchHistoryController(IWordService wordService, IUserLogService userLogService,
            ICachedWordService cachedWordService)
        {
            _cachedWordService = cachedWordService;
            _userLogService = userLogService;
            _wordService = wordService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var logs = await _userLogService.GetAllSolverLogs();

                List<SearchHistory> history = new List<SearchHistory>();
                foreach (var log in logs)
                {
                    List<string> anagrams = new List<string>();
                    var cached = await _cachedWordService.GetSelectedCachedWord(log.SearchPhrase);
                    if (cached != null)
                    {
                        var anagramsIds = cached.AnagramsIds.Split(';').ToList();

                        foreach (var wordId in anagramsIds)
                        {
                            var phrase = wordId.Split('/').ToList();
                            string wordFound = "";
                            foreach (var word in phrase)
                            {
                                var idToGet = int.Parse(word);
                                var anagram = await _wordService.GetWordById(idToGet);
                                wordFound += anagram.Word + " ";
                            }
                            anagrams.Add(wordFound);
                        }
                    }
                    history.Add(
                        new SearchHistory
                        {
                            Anagrams = anagrams,
                            Ip = log.Ip,
                            SearchPhrase = log.SearchPhrase,
                            SearchTime = log.SearchTime
                        });
                }

                return View(history);
            }
            catch(Exception ex)
            {
                @ViewData["Error"] = ex.Message;
                return View();
            }
        }
    }
}
