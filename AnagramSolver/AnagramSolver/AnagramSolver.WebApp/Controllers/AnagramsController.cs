using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Entities;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    public class AnagramsController : Controller
    {
        private readonly ICookiesHandler _cookiesHandler;
        private readonly IWordService _wordService;

        public AnagramsController(ICookiesHandler cookiesHandler, IWordService wordService)
        {
            _cookiesHandler = cookiesHandler;
            _wordService = wordService;
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
                ModelState.AddModelError("", ex.Message);
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
                ModelState.AddModelError("", ex.Message);
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
                await _wordService.InsertWord(anagram);
                
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(anagram);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
