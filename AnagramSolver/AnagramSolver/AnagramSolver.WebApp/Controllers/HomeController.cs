using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.WebApp.Models;
using AnagramSolver.Contracts.Interfaces;
using System.Linq;
using AnagramSolver.Contracts.Models;
using AnagramSolver.BusinessLogic.Database;
using System.Collections.Generic;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IUserInterface _userInterface;
        private readonly ICookiesHandler _cookiesHandler;
        private readonly CachedWordQueries _cachedWord;
        private readonly UserLogQueries _userLog;
        private readonly WordQueries _wordQueries;

        public HomeController(IUserInterface userInterface, IAnagramSolver anagramSolver,
            ICookiesHandler cookiesHandler, WordQueries wordQuerie, UserLogQueries logQueries,
            CachedWordQueries cachedWordQueries)
        {
            _userInterface = userInterface;
            _anagramSolver = anagramSolver;
            _cookiesHandler = cookiesHandler;
            _cachedWord = cachedWordQueries;
            _userLog = logQueries;
            _wordQueries = wordQuerie;
        }

        public IActionResult Index(string id)
        {
            try
            { 
                var input = _userInterface.ValidateInputData(id);
                if (string.IsNullOrEmpty(input))
                    throw new Exception("You must enter at least one word");

                /*  var cookieValue = _cookiesHandler.GetCookieByKey(input);
                  if (!string.IsNullOrEmpty(cookieValue))
                      return View(cookieValue.Split(';').ToList());*/

                IList<string> anagrams = new List<string>();

                var cachedWord = _cachedWord.GetCachedWord(input);
                if (cachedWord != null)
                {
                    var anagramsIds = cachedWord.AnagramsIds.Split(';').ToList();
                    
                    foreach (var wordId in anagramsIds)
                        anagrams.Add(_wordQueries.SelectWordById(wordId));
                    return View(anagrams);
                }


                Stopwatch sw = new Stopwatch();
                sw.Start();
                anagrams = _anagramSolver.GetAnagrams(input);
                sw.Stop();

                _userLog.InsertLog(new UserLog(GetUserIp(), id, sw.Elapsed));

                //removing input element
                anagrams.Remove(id);



                //_cookiesHandler.AddCookie(id, string.Join(";", anagrams.ToArray()));

                return View(anagrams);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(id);
            }
        }

        public IActionResult DisplayCookies()
        {
            var cookies = _cookiesHandler.GetCurrentCookies();
            return View(cookies);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetUserIp()
        {
            var ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
            return ip;
        }
    }
}
