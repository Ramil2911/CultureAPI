using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Images.Models;
using MovieWebsite.Images.Models.Databases;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using MovieWebsite.Shared;

namespace MovieWebsite.Images.Controllers
{
    [Route("image")]
    public class ImageController : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            await using var db = new ImageContext();
            var image = await db.Images.FirstOrDefaultAsync(x => x.Id == id);
            return File(image.ImageData, image.ImageType, image.ImageTitle);
        }
        
        [HttpPost("addFile")]
        [Authorize]
        public async Task<IActionResult> AddImageFile(IFormFile file)
        {
            await using var db = new ImageContext();
            var length = file.Length;
            switch (length)
            {
                case <= 0:
                    return Json(new { success = 0 });
                case > 4194304:
                    return Json(new { success = 0 });
            }

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
            return Json(new {success = 1, file=new {url=ServerIps.Value[4]+"/image/"+image.Id}});
        }
        
        [HttpPost("addUrl")]
        [Authorize]
        public async Task<IActionResult> AddImageUrl(string url)
        {
            using var client = new WebClient();
            var file = await client.DownloadDataTaskAsync(url);
            
            await using var db = new ImageContext();
            var length = file.Length;
            switch (length)
            {
                case <= 0:
                    return Json(new { success = 0 });
                case > 4194304:
                    return Json(new { success = 0 });
            }

            var image = new Image
            {
                ImageData = file,
                ImageTitle = "file",
                ImageType = "application/octet-stream" //TODO: try to implement MIME detector
            };
            await db.Images.AddAsync(image);
            await db.SaveChangesAsync();
            return Json(new {success = 1, file=new {url=ServerIps.Value[4]+"/image/"+image.Id}});
        }
    }
}