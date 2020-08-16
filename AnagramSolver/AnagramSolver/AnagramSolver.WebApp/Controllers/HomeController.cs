using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly ICookiesHandlerService _cookiesHandler;
        private readonly ICachedWordService _cachedWordService;
        private readonly IUserLogService _userLogService;
        private readonly IWordService _wordService;
        private readonly AnagramSolverCodeFirstContext _context;

        public HomeController(IAnagramSolver anagramSolver, ICookiesHandlerService cookiesHandler,
            IUserLogService logService, ICachedWordService cachedWordService,
            IWordService wordService, AnagramSolverCodeFirstContext context)
        {
            _anagramSolver = anagramSolver;
            _cookiesHandler = cookiesHandler;
            _cachedWordService = cachedWordService;
            _userLogService = logService;
            _wordService = wordService;
            _context = context;
        }

        public async Task<IActionResult> Index([Required] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return View();

                IList<string> anagrams = new List<string>();

                var solvesLeft = await _userLogService.CountAnagramsLeftForIpToSolve();
                if (solvesLeft <= 0)
                    throw new Exception("You reached your solve limit. Add new word to increase your limit");

                var cachedWord = await _cachedWordService.GetSelectedCachedWord(id);
                if (cachedWord != null)// || cachedWord != DBNull.Value)
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
                    return View(anagrams);
                }


                Stopwatch sw = new Stopwatch();
                sw.Start();
                anagrams = await _anagramSolver.GetAnagrams(id);
                sw.Stop();

                await _userLogService.AddLog(sw.Elapsed, UserActionTypes.GetAnagrams, id);

                //removing input element
                anagrams.Remove(id);

                _cookiesHandler.AddCookie(id, string.Join(";", anagrams.ToArray()));

                await _context.SaveChangesAsync();
                return View(anagrams);

            }
            catch (Exception ex)
            {
                @ViewData["Error"] = ex.Message;
                return View();
            }
        }

        public IActionResult DisplayCookies()
        {
            try
            {
                var cookies = _cookiesHandler.GetCurrentCookies();
                if (cookies.Count < 1)
                    return View();

                return View(cookies);
            }
            catch(Exception ex)
            {
                @ViewData["Error"] = ex.Message;
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
