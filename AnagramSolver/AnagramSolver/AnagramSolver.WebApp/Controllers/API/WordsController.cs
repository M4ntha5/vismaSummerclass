using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces.Services;
using AnagramSolver.Contracts.Models;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers.API
{
    [Route("api/words")]
    [ApiController]
    public class WordsController : Controller
    {
        private readonly IWordService _wordService;
        private readonly AnagramSolverCodeFirstContext _context;

        public WordsController(IWordService wordService, AnagramSolverCodeFirstContext context)
        {
            _wordService = wordService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber, int pageSize = 100)
        {
            var wordsList = await _wordService.GetAllWords();
            return Ok(PaginatedList<Anagram>.Create(wordsList, pageNumber ?? 1, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var word = await _wordService.GetWordById(id);
            return Ok(word);
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Post(string word, string category)
        {
            var anagram = new Anagram() { Category = category, Word = word };
            await _wordService.InsertWord(anagram);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> Update(int id, string word, string category)
        {
            var anagram = new Anagram() { Category = category, Word = word };
            await _wordService.UpdateWord(id, anagram);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _wordService.DeleteWordById(id);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
