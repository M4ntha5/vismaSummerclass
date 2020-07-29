using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    [Route("api/anagrams")]
    [ApiController]
    public class AnagramsAPIController : ControllerBase
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IUserInterface _userInterface;

        public AnagramsAPIController(IUserInterface userInterface, IAnagramSolver anagramSolver)
        {
            _userInterface = userInterface;
            _anagramSolver = anagramSolver;
        }

        [HttpGet("{word}")]
        public IActionResult Get(string word)
        {
            try
            {
                if (string.IsNullOrEmpty(word))
                    throw new Exception("You must enter at least one word");

                var input = _userInterface.ValidateInputData(word);

                if (string.IsNullOrEmpty(input))
                    throw new Exception("You must enter at least one word");

                var anagrams = _anagramSolver.GetAnagrams(input);

                //removing input element
                anagrams.Remove(word);

                return Ok(anagrams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
