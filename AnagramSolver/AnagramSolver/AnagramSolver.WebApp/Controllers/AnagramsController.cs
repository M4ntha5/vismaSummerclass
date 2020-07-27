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

        public IActionResult Index()
        {
            var allData = _fileRepository.ReadDataFromFile();
            if (allData.Count == 0 || allData == null)
                return View();
            return View(allData);
        }

        // GET: Anagrams/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
                return NotFound();

            var anagrams = _fileRepository.GetSelectedWordAnagrams(id);

            if (anagrams == null)
                return NotFound();

            return View(anagrams);
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
            if (ModelState.IsValid)
            {
                //_fileRepository.Add(anagram);
                return RedirectToAction(nameof(Index));
            }
            return View(anagram);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
