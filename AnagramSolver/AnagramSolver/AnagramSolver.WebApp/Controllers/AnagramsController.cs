﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using AnagramSolver.BusinessLogic;
using AnagramSolver.BusinessLogicDB.Database;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    public class AnagramsController : Controller
    {
        private readonly IWordRepository _fileRepository;
        private readonly ICookiesHandler _cookiesHandler;

        public AnagramsController(IWordRepository fileRepository, ICookiesHandler cookiesHandler)
        {
            _fileRepository = fileRepository;
            _cookiesHandler = cookiesHandler;
        }

        public IActionResult Index(int? pageNumber, string phrase = null, int pageSize = 100)
        {
            try
            {
                List<Anagram> result;
                if (!string.IsNullOrEmpty(phrase))
                {
                    var db = new WordQueries();
                    result = db.SelectWordsBySearch(phrase);
                    pageSize = result.Count;
                }
                else
                    result = _fileRepository.GetWords();

                return View(PaginatedList<Anagram>.Create(result, pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Anagrams/Details/5
        public IActionResult Details(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Selected word do not exist");

                var anagrams = _fileRepository.GetSelectedWordAnagrams(id);

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
        public IActionResult Create([Bind("Word,Case")] Anagram anagram)
        {
            try
            {
                if (string.IsNullOrEmpty(anagram.Word) || string.IsNullOrEmpty(anagram.Case))
                    throw new Exception("You must fill all the fields");

                _cookiesHandler.ClearAllCookies();
                _fileRepository.AddWordToFile(anagram);
                
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
