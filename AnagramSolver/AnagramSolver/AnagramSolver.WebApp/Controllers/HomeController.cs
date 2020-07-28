using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.WebApp.Models;
using AnagramSolver.Contracts.Interfaces;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IUserInterface _userInterface;

        public HomeController(IUserInterface userInterface, IAnagramSolver anagramSolver)
        {
            _userInterface = userInterface;
            _anagramSolver = anagramSolver;
        }

        public IActionResult Index(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("You must enter at least one word");

                var input = _userInterface.ValidateInputData(id);

                if (string.IsNullOrEmpty(input))
                    throw new Exception("You must enter at least one word");

                var anagrams = _anagramSolver.GetAnagrams(input);

                //removing input element
                anagrams.Remove(id);

                return View(anagrams);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(id);
            }
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
