using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnagramSolver.WebApp.Controllers
{
    public class AnagramsController : Controller
    {
        private readonly ILogger<AnagramsController> _logger;
        private readonly IWordRepository _fileRepository;

        public AnagramsController(ILogger<AnagramsController> logger, IWordRepository fileRepository)
        {
            _logger = logger;
            _fileRepository = fileRepository;
        }

        public IActionResult Index(int? pageNumber)
        {
            try
            {
                var words = _fileRepository.GetWords();
                if (words.Count == 0 || words == null)
                    return View();

                int pageSize = 100;
                return View(PaginatedList<Anagram>.Create(words, pageNumber ?? 1, pageSize));
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
                if (id == null)
                    return NotFound();

                var anagrams = _fileRepository.GetSelectedWordAnagrams(id);

                if (anagrams == null)
                    return NotFound();

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

                if (ModelState.IsValid)
                {
                    _fileRepository.AddWordToFile(anagram);
                    return RedirectToAction(nameof(Index));
                }
                return View(anagram);
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
