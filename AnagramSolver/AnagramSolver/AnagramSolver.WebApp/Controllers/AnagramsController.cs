using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AnagramSolver.WebApp.Controllers
{
    public class AnagramsController : Controller
    {
        private readonly ICookiesHandlerServvice _cookiesHandler;
        private readonly IWordService _wordService;
        private readonly IUserLogRepository _userLogRepository;

        public AnagramsController(ICookiesHandlerServvice cookiesHandler, IWordService wordService,
            IUserLogRepository userLogRepository)
        {
            _cookiesHandler = cookiesHandler;
            _wordService = wordService;
            _userLogRepository = userLogRepository;
        }

        public async Task<IActionResult> Index(int? pageNumber, string phrase = null, int pageSize = 100)
        {
            try
            {
                List<Anagram> result;
                if (!string.IsNullOrEmpty(phrase))
                {
                    result = await _wordService.GetWordsBySearch(phrase);
                    pageSize = result.Count;
                }
                else
                    result = await _wordService.GetAllWords();

                return View(PaginatedList<Anagram>.Create(result, pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                @ViewData["Error"] = ex.Message;
                return View();
            }
        }

        // GET: Anagrams/Details/5
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Selected word do not exist");

                var anagrams = await _wordService.GetWordAnagrams(id);

                if (anagrams == null || anagrams.Count == 0)
                    return RedirectToAction(nameof(Index));

                return View(anagrams);
            }
            catch (Exception ex)
            {
                @ViewData["Error"] = ex.Message;
                return View(id);
            }
        }

        // GET: Anagrams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Anagrams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Word,Case")] Anagram anagram)
        {
            try
            {
                if (string.IsNullOrEmpty(anagram.Word) || string.IsNullOrEmpty(anagram.Case))
                    throw new Exception("You must fill all the fields");

                _cookiesHandler.ClearAllCookies();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                await _wordService.InsertWord(anagram);
                sw.Stop();

                await _userLogRepository.InsertLog(
                    new UserLog(GetUserIp(), null, sw.Elapsed, UserActionTypes.InsertWord.ToString()));

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                @ViewData["Error"] = ex.Message;
                return View(anagram);
            }
        }

        // GET: Anagrams/Update/5
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id == null)
                    return View();

                var word = await _wordService.GetWordById((int)id);

                if (word == null)
                    return View();

                return View(word);
            }
            catch(Exception ex)
            {
                @ViewData["Error"] = ex.Message;
                return View();
            }
        }

        // POST: Anagrams/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("ID,Word,Case")] Anagram anagram)
        {
            try
            {
                if (id != anagram.ID)
                    return View(anagram);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                await _wordService.UpdateWord(id, anagram);
                sw.Stop();

                await _userLogRepository.InsertLog(
                        new UserLog(GetUserIp(), null, sw.Elapsed, UserActionTypes.UpdateWord.ToString()));

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                @ViewData["Error"] = ex.Message;
                return View(anagram);
            }
        }

        // GET: Anagrams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return View();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                await _wordService.DeleteWordById((int)id);
                sw.Stop();

                await _userLogRepository.InsertLog(
                        new UserLog(GetUserIp(), null, sw.Elapsed, UserActionTypes.DeleteWord.ToString()));

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                @ViewData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
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
