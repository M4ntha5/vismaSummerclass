using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly ICookiesHandlerServvice _cookiesHandler;
        private readonly ICachedWordRepository _cachedWordRepository;
        private readonly IUserLogRepository _userLogRepository;
        private readonly IWordService _wordService;

        public HomeController(IAnagramSolver anagramSolver, ICookiesHandlerServvice cookiesHandler,
            IUserLogRepository logRepository, ICachedWordRepository cachedWordRepository,
            IWordService wordService)
        {
            _anagramSolver = anagramSolver;
            _cookiesHandler = cookiesHandler;
            _cachedWordRepository = cachedWordRepository;
            _userLogRepository = logRepository;
            _wordService = wordService;
        }

        public async Task<IActionResult> Index(string id)
        {
            try
            {
                /*  var cookieValue = _cookiesHandler.GetCookieByKey(input);
                  if (!string.IsNullOrEmpty(cookieValue))
                      return View(cookieValue.Split(';').ToList());*/

                if (string.IsNullOrEmpty(id))
                    return View();

                IList<string> anagrams = new List<string>();

                var cachedWord = await _cachedWordRepository.GetCachedWord(id);
                if (cachedWord != null)// || cachedWord != DBNull.Value)
                {
                    var anagramsIds = cachedWord.AnagramsIds.Split(';').ToList();

                    foreach (var wordId in anagramsIds)
                    {
                        var phrase = wordId.Split('/').ToList();
                        string wordFound = "";
                        foreach (var word in phrase)
                        {
                            wordFound += await _wordService.GetWordById(word) + " ";
                        }
                        anagrams.Add(wordFound);
                    }
                    return View(anagrams);
                }


                Stopwatch sw = new Stopwatch();
                sw.Start();
                anagrams = await _anagramSolver.GetAnagrams(id);
                sw.Stop();

                await _userLogRepository.InsertLog(new UserLog(GetUserIp(), id, sw.Elapsed));

                //removing input element
                anagrams.Remove(id);

                _cookiesHandler.AddCookie(id, string.Join(";", anagrams.ToArray()));

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
