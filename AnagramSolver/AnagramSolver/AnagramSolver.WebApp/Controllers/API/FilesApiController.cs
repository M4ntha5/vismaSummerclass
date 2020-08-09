using AnagramSolver.BusinessLogic.Properties;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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

