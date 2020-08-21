using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.EF.CodeFirst;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AnagramSolver.WebApp.Controllers
{
    [Route("api/anagrams")]
    [ApiController]
    public class AnagramsAPIController : ControllerBase
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly AnagramSolverCodeFirstContext _context;

        public AnagramsAPIController(IAnagramSolver anagramSolver, AnagramSolverCodeFirstContext context)
        {
            _anagramSolver = anagramSolver;
            _context = context;
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> Get(string word)
        {
            try
            {
                if (string.IsNullOrEmpty(word))
                    throw new Exception("You must enter at least one word");

                var anagrams = await _anagramSolver.GetAnagrams(word);

                //removing input element
                anagrams.Remove(word);

                //saves inserted cached word
                await _context.SaveChangesAsync();

                return Ok(anagrams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
