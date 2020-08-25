using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers.API
{
    [Route("api/words")]
    [ApiController]
    public class WordsController : Controller
    {
        private readonly IWordService _wordService;

        public WordsController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber, int pageSize = 100)
        {
            var wordsList = await _wordService.GetAllWords();
            return Ok(PaginatedList<Anagram>.Create(wordsList, pageNumber ?? 1, pageSize));
        }

        [HttpGet("{word}")]
        public async Task<IActionResult> Get(string word)
        {
            var anagrams = await _wordService.GetWordAnagrams(word);
            return Ok(anagrams);
        }
    }
}
