using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AnagramSolver.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesApiController : ControllerBase
    {
        private readonly IWordRepository _wordRepository;

        public FilesApiController(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        [HttpGet]
        [Route("dictionary")]
        public ActionResult GetDictionaryFile()
        {
            var path = _wordRepository.GetDataFilePath();
            var fileName = path.Split("\\")[^1];
            
            return File(System.IO.File.OpenRead(path), "application/txt", fileName); ;
        }
    }
}

