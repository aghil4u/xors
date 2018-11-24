using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Image")]
    public class ImageUploadController : Controller
    {
        private readonly IHostingEnvironment _environment;
        public ImageUploadController(IHostingEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }
        // POST: api/Image
//        [HttpPost]
//        public async Task Post(IFormFile file)
//        {
//            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
//            if (file.Length > 0)
//            {
//                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
//                {
//                    await file.CopyToAsync(fileStream);
//                }
//            }
//        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
 
                if (file.Length > 0)
                {
                    long size = file.Length;

                    var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                    var filePath = Path.Combine(uploads, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Ok(new { count = 1, size, filePath });
            }
                else
                {
                    return null;
                }
   
         
        }
    }
}