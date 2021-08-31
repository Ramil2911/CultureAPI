using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Images.Models;
using MovieWebsite.Images.Models.Databases;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using MovieWebsite.Shared;

namespace MovieWebsite.Images.Controllers
{
    /// <summary>
    /// Controller with image endpoints
    /// </summary>
    [Route("image")]
    public class ImageController : Controller
    {
        /// <summary>
        /// Get saved image
        /// </summary>
        /// <param name="guid">Guid of image</param>
        /// <returns>File</returns>
        /// <response code="200">Success</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{guid:Guid}")]
        public async Task<IActionResult> GetImage(Guid guid)
        {
            await using var db = new ImageContext();
            var image = await db.Images.FirstOrDefaultAsync(x => x.Guid == guid);
            if (image is null) return NotFound();
            return File(image.ImageData, image.ImageType, image.ImageTitle);
        }
        
        /// <summary>
        /// Save image
        /// </summary>
        /// <param name="file">Image to save</param>
        /// <returns>Json with success and file url or json with failure</returns>
        [ProducesResponseType(200, Type = typeof(ImageResponseBody))]
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
            return Json(new ImageResponseBody() {success = 1, file=new ImageResponseBody.UrlContainer() {url=ServerIps.Value[4]+"/image/"+image.Guid}});
        }
        
        /// <summary>
        /// Adds image by its url
        /// </summary>
        /// <param name="url">Url of image</param>
        /// <returns>Json with success and file url or json with failure</returns>
        [ProducesResponseType(200, Type = typeof(ImageResponseBody))]
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
            return Json(new ImageResponseBody() {success = 1, file=new ImageResponseBody.UrlContainer() {url=ServerIps.Value[4]+"/image/"+image.Guid}});
        }
        
        /// <summary>
        /// [INTERNAL] Checks does image exist
        /// </summary>
        /// <param name="guid">Guid of image</param>
        /// <returns>StatusCode</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Internal]
        [HttpGet("check")]
        public async Task<IActionResult> ImageExists(Guid guid)
        {
            await using var db = new ImageContext();
            return db.Images.AsNoTracking().Any(x => x.Guid == guid) ? Ok() : NotFound();
        }

        /// <summary>
        /// Response body for image fet endpoint
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public class ImageResponseBody
        {
            /// <summary>
            /// Status code: 1 for success, 0 for fail
            /// </summary>
            public int success { get; set; }
            /// <summary>
            /// Container with file url
            /// </summary>
            public UrlContainer file { get; set; }
            public class UrlContainer
            {
                public string url { get; set; }
            }
        }
    }
}