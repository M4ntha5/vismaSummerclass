using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.WebApp.Models;
using AnagramSolver.Contracts.Interfaces;
using System.Linq;
using AnagramSolver.Contracts.Models;
using AnagramSolver.BusinessLogic.Database;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AnagramSolver.EF.DatabaseFirst.Data;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly ICookiesHandler _cookiesHandler;
        private readonly CachedWordQueries _cachedWord;
        private readonly UserLogQueries _userLog;
        private readonly WordQueries _wordQueries;

        public HomeController(IAnagramSolver anagramSolver, ICookiesHandler cookiesHandler, 
            WordQueries wordQuerie, UserLogQueries logQueries, CachedWordQueries cachedWordQueries)
        {
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
                /*  var cookieValue = _cookiesHandler.GetCookieByKey(input);
                  if (!string.IsNullOrEmpty(cookieValue))
                      return View(cookieValue.Split(';').ToList());*/

                IList<string> anagrams = new List<string>();

                var cachedWord = _cachedWord.GetCachedWord(id);
                if (cachedWord != null)
                {
                    var anagramsIds = cachedWord.AnagramsIds.Split(';').ToList();
                    
                    foreach (var wordId in anagramsIds)
                    {
                        var phrase = wordId.Split('/').ToList();
                        string wordFound = "";
                        foreach(var word in phrase)
                        {
                            wordFound += _wordQueries.SelectWordById(word) + " ";
                        }
                        anagrams.Add(wordFound);
                    }
                    return View(anagrams);
                }


                Stopwatch sw = new Stopwatch();
                sw.Start();
                anagrams = _anagramSolver.GetAnagrams(id);
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
