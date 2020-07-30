using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.WebApp.Models;
using AnagramSolver.Contracts.Interfaces;
using System.Linq;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IUserInterface _userInterface;
        private readonly ICookiesHandler _cookiesHandler;

        public HomeController(IUserInterface userInterface, IAnagramSolver anagramSolver,
            ICookiesHandler cookiesHandler)
        {
            _userInterface = userInterface;
            _anagramSolver = anagramSolver;
            _cookiesHandler = cookiesHandler;
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

                var cookieValue = _cookiesHandler.GetCookieByKey(input);
                if (!string.IsNullOrEmpty(cookieValue))
                    return View(cookieValue.Split(';').ToList());        

               
                var anagrams = _anagramSolver.GetAnagrams(input);

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
    }
}
