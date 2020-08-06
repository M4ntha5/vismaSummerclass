﻿using System;
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
using AnagramSolver.BusinessLogic;
using AnagramSolver.BusinessLogic.Properties;

namespace AnagramSolver.WebApp.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesApiController : ControllerBase
    {
        [HttpGet]
        [Route("dictionary")]
        public ActionResult GetDictionaryFile()
        {
            var path = Path.GetTempPath() + "zodynas.txt";

            System.IO.File.WriteAllText(path, Resources.zodynas);
            return File(System.IO.File.OpenRead(path), "application/txt", "zodynas.txt");
        }
    }
}

