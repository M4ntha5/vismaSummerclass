using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.BusinessLogicDB.Database;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    public class SearchHistoryController : Controller
    {
        private readonly CachedWordQueries _cachedWord;
        private readonly UserLogQueries _userLog;
        private readonly WordQueries _wordQueries;

        public SearchHistoryController()
        {
            _cachedWord = new CachedWordQueries();
            _userLog = new UserLogQueries();
            _wordQueries = new WordQueries();
        }


        public IActionResult Index()
        {
            var logs = _userLog.GetAllLogs();

            List<SearchHistory> history = new List<SearchHistory>();
            foreach(var log in logs)
            {
                List<string> anagrams = new List<string>();
                var cached = _cachedWord.GetCachedWord(log.SearchPhrase);
                if(cached != null)
                {
                    var anagramsIds = cached.Anagrams.Split(';').ToList();

                    foreach (var wordId in anagramsIds)
                        anagrams.Add(_wordQueries.SelectWordById(wordId));
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
    }
}
