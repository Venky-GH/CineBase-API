using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineBaseV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        [HttpPost]
        public IActionResult Post(IFormFile file)
        {
            try
            {
                var filePath = "./images/" + file.FileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok(new
                {
                    message = "Image uploaded successfully",
                    result = file.FileName
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                return StatusCode(500, new
                {
                    message = "Something went wrong"
                });
            }
        }

        [HttpGet("{ImageName}")]
        public IActionResult Get(string ImageName)
        {
            try
            {
                var path = Path.GetFullPath("./images/" + ImageName);
                var extension = ImageName.Split(".")[ImageName.Split(".").Length - 1];
                var imageFileStream = System.IO.File.OpenRead(path);
                return File(imageFileStream, "image/" + extension);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return NotFound(new
                {
                    message = "Image not found"
                });
            }
        }
    }
}