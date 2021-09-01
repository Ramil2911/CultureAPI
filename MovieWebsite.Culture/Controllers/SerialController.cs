using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Movies.Models;
using MovieWebsite.Movies.Models.Databases;
using MovieWebsite.Shared;

namespace MovieWebsite.Movies.Controllers
{
    [Route("culture/serials")]
    public class SerialController : Controller
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FetchSerial(int id)
        {
            await using var db = new CultureContext();
            var serial = await db.Serials
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (serial is null) return NotFound("Requested serial not found");
            return Json(serial);
        }

        /// <summary>
        /// Adds Serial to database
        /// </summary>
        /// <returns>Returns id of added Serial if success</returns>
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> AddSerial([FromBody] SerialRequestBody body)
        {
            await using var db = new CultureContext();

            var errorBuilder = new StringBuilder("");
            if (body.Name is null) errorBuilder.Append("Name data not found;\n");
            if (body.Characters is null) errorBuilder.Append("Characters data not found;\n");
            if (body.PosterId is null) errorBuilder.Append("Poster data not found;\n");
            if (body.Description is null) errorBuilder.Append("Description data not found;\n");
            if (body.Directors is null) errorBuilder.Append("Directors data not found;\n");
            if (body.Actors is null) errorBuilder.Append("Actors data not found;\n");
            if (errorBuilder.Length != 0) return BadRequest(errorBuilder.ToString());

            var serial = new Serial
            {
                Name = body.Name!,
                PosterId = body.PosterId!.Value,
                Description = body.Description!,
                Directors = await db.Persons.Where(x=>body.Directors!.Contains(x.Id)).ToArrayAsync(),
                Characters = await db.Characters.Where(x=>body.Characters!.Contains(x.Id)).ToArrayAsync(),
                Actors = await db.Persons.Where(x=>body.Actors!.Contains(x.Id)).ToArrayAsync(),
                Genres = body.Genres,
                OriginalName = body.OriginalName
            };
            if (body.FranchiseId.HasValue)
                serial.Franchise = await db.Franchises.FirstOrDefaultAsync(x => body.FranchiseId!.Value == x.Id);
            else
                serial.Franchise = null;
            await db.Serials.AddAsync(serial);
            
            await db.SaveChangesAsync();
            return Ok(new {id=serial.Id});
        }

        /// <summary>
        /// Updates Serial in database
        /// </summary>
        /// <returns>Returns id of added Serial if success, set null if you dont want to update</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateSerial(int id, [FromBody] SerialRequestBody body)
        {
            //this function should not be run often, so efficiency is not important
            await using var db = new CultureContext();
            var serial = await db.Serials
                .FirstOrDefaultAsync(x => x.Id == id);
            if (serial is null) return BadRequest("Serial not found");
            
            if (body.OriginalName is not null)
                serial.OriginalName = body.OriginalName;
            if (body.Name is not null)
                serial.Name = body.Name;
            if (body.Description is not null)
                serial.Description = body.Description;
            if (body.Directors is not null && body.Directors.Count > 0)
                serial.Directors = await db.Persons.Where(x => body.Directors.Contains(x.Id)).ToArrayAsync();
            if (body.Characters is not null && body.Characters.Count > 0)
                serial.Characters = await db.Characters.Where(x => body.Characters.Contains(x.Id)).ToArrayAsync();
            if (body.Actors is not null && body.Actors.Count > 0)
                serial.Actors = await db.Persons.Where(x => body.Actors.Contains(x.Id)).ToArrayAsync();
            if (body.Genres is not null && body.Genres.Count > 0)
                serial.Genres = body.Genres;
            if (body.FranchiseId.HasValue)
                serial.Franchise = await db.Franchises.FirstOrDefaultAsync(x => x.Id == body.FranchiseId);
            

            await db.SaveChangesAsync();
            return Ok(new {id=serial.Id});
        }
        
        /// <summary>
        /// Deletes Serial from database
        /// </summary>
        /// <param name="id">Id of Serial to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete()]
        public async Task<IActionResult> RemoveSerial(int id)
        {
            await using var db = new CultureContext();
            var serial = await db.Serials.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (serial is null) return NotFound();
            db.Serials.Remove(serial);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("popular")]
        public async Task<IActionResult> FetchSerials(uint lenght, uint skip)
        {
            await using var db = new CultureContext();
            var serials = await db.Serials.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(serials);
        }

        public class SerialRequestBody
        {
            public Guid? PosterId { get; set; }
            public string? Description { get; set; }
            public string? Name { get; set; }
            public string? OriginalName { get; set; }
            public HashSet<Genre>? Genres { get; set; } = new HashSet<Genre>();
            public int? FranchiseId { get; set; }
            public ICollection<int>? Directors { get; set; } = new List<int>();
            public ICollection<int>? Actors { get; set; } = new List<int>();
            public ICollection<int>? Characters { get; set; } = new List<int>();
        }
    }
}