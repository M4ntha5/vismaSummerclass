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
        private readonly IUserLogService _userLogService;
        private readonly ISearchHistoryService _searchHistoryService;

        public SearchHistoryController(
            IUserLogService userLogService, ISearchHistoryService searchHistoryService)
        {
            _userLogService = userLogService;
            _searchHistoryService = searchHistoryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {           
                var logs = await _userLogService.GetAllSolverLogs();

                var history = new List<SearchHistory>();
                foreach (var log in logs)
                {
                    var anagrams = await _searchHistoryService.GetSearchedAnagrams(log.SearchPhrase);
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
