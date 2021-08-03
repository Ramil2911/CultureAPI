using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Images.Models;
using MovieWebsite.Images.Models.Databases;
using System;

namespace MovieWebsite.Images.Controllers
{
    [Route("image")]
    public class ImageController : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(long id)
        {
            await using var db = new ImageContext();
            var image = await db.Images.FirstOrDefaultAsync(x => x.Id == id);
            return File(image.ImageData, image.ImageType, image.ImageTitle);
        }
        
        [HttpPut("add")]
        [Authorize]
        public async Task<IActionResult> AddImage(long id, IFormFile file)
        {
            await using var db = new ImageContext();
            var length = file.Length;
            if (length < 0)
                return BadRequest();

            await using var fileStream = file.OpenReadStream();
            var bytes = new byte[length];
            await fileStream.ReadAsync(bytes.AsMemory(0, (int)file.Length));
            var image = new Image
            {
                ImageData = bytes,
                ImageTitle = file.Name,
                ImageType = file.ContentType
            };
            await db.Images.AddAsync(image);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}