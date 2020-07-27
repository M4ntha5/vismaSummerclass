using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnagramSolver.WebApp.Models;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.BusinessLogic.Repositories;
using AnagramSolver.Console.UI;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnagramSolver _anagramSolver;
        private readonly UserInterface _userInterface;

        public HomeController(ILogger<HomeController> logger, IAnagramSolver anagramSolver)
        {
            _logger = logger;
            _userInterface = new UserInterface();
            _anagramSolver = anagramSolver;
        }

        public IActionResult Index(string id)
        {
            if(string.IsNullOrEmpty(id))
                return View();

            var input = _userInterface.ValidateInputData(id);
            var anagrams = _anagramSolver.GetAnagrams(input);

            //removing input element
            anagrams.Remove(id);

            return View(anagrams);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
