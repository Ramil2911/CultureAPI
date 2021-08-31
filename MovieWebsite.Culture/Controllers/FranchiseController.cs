using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Movies.Models;
using MovieWebsite.Movies.Models.Databases;

namespace MovieWebsite.Movies.Controllers
{
    [Route("culture/franchises")]
    public class FranchiseController : Controller
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FetchFranchise(int id)
        {
            await using var db = new CultureContext();
            var franchise = await db.Franchises
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (franchise is null) return NotFound("Requested franchise not found");
            return Json(franchise);
        }

        /// <summary>
        /// Adds Franchise to database
        /// </summary>
        /// <returns>Returns id of added Franchise if success</returns>
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> AddFranchise([FromBody] FranchiseRequestBody body)
        {
            await using var db = new CultureContext();

            var errorBuilder = new StringBuilder("");
            if (body.Name is null) errorBuilder.Append("Name data not found;\n");
            if (body.Books is null) errorBuilder.Append("Books data not found;\n");
            if (body.Characters is null) errorBuilder.Append("Characters data not found;\n");
            if (body.Companies is null) errorBuilder.Append("Companies data not found;\n");
            if (body.Games is null) errorBuilder.Append("Games data not found;\n");
            if (body.Movies is null) errorBuilder.Append("Movies data not found;\n");
            if (body.Serials is null) errorBuilder.Append("Serials data not found;\n");
            if (errorBuilder.Length != 0) return BadRequest(errorBuilder.ToString());

            var franchise = new Franchise
            {
                Name = body.Name!,
                Books = await db.Books.Where(x=>body.Books!.Contains(x.Id)).ToArrayAsync(),
                Characters = await db.Characters.Where(x=>body.Characters!.Contains(x.Id)).ToArrayAsync(),
                Companies = await db.Companies.Where(x=>body.Companies!.Contains(x.Id)).ToArrayAsync(),
                Games = await db.Games.Where(x=>body.Games!.Contains(x.Id)).ToArrayAsync(),
                Movies = await db.Movies.Where(x=>body.Movies!.Contains(x.Id)).ToArrayAsync(),
                Serials = await db.Serials.Where(x=>body.Serials!.Contains(x.Id)).ToArrayAsync(),
            };
            await db.Franchises.AddAsync(franchise);
            
            await db.SaveChangesAsync();
            return Ok(new {id=franchise.Id});
        }

        /// <summary>
        /// Updates Franchise in database
        /// </summary>
        /// <returns>Returns id of added Franchise if success, set null if you dont want to update</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateFranchise(int id, [FromBody] FranchiseRequestBody body)
        {
            //this function should not be run often, so efficiency is not important
            await using var db = new CultureContext();
            var franchise = await db.Franchises
                .FirstOrDefaultAsync(x => x.Id == id);
            if (franchise is null) return BadRequest("Franchise not found");
            
            if (body.Name is not null)
                franchise.Name = body.Name;
            if (body.Movies is not null && body.Movies.Count > 0)
                franchise.Movies = await db.Movies.Where(x => body.Movies.Contains(x.Id)).ToArrayAsync();
            if (body.Serials is not null && body.Serials.Count > 0)
                franchise.Serials = await db.Serials.Where(x => body.Movies.Contains(x.Id)).ToArrayAsync();
            if (body.Books is not null && body.Books.Count > 0)
                franchise.Books = await db.Books.Where(x => body.Books.Contains(x.Id)).ToArrayAsync();
            if (body.Games is not null && body.Games.Count > 0)
                franchise.Games = await db.Games.Where(x => body.Games.Contains(x.Id)).ToArrayAsync();
            if (body.Characters is not null && body.Characters.Count > 0)
                franchise.Characters = await db.Characters.Where(x => body.Characters.Contains(x.Id)).ToArrayAsync();
            if (body.Companies is not null && body.Companies.Count > 0)
                franchise.Companies = await db.Companies.Where(x => body.Companies.Contains(x.Id)).ToArrayAsync();

            await db.SaveChangesAsync();
            return Ok(new {id=franchise.Id});
        }
        
        /// <summary>
        /// Deletes Franchise from database
        /// </summary>
        /// <param name="id">Id of Franchise to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete()]
        public async Task<IActionResult> RemoveFranchise(int id)
        {
            await using var db = new CultureContext();
            var franchise = await db.Franchises.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (franchise is null) return NotFound();
            db.Franchises.Remove(franchise);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("popular")]
        public async Task<IActionResult> FetchFranchises(uint lenght, uint skip)
        {
            await using var db = new CultureContext();
            var franchises = await db.Games.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(franchises);
        }

        public class FranchiseRequestBody
        {
            public string? Name { get; set; } = "";
            public ICollection<int>? Movies { get; set; } = new List<int>();
            public ICollection<int>? Serials { get; set; } = new List<int>();
            public ICollection<int>? Books { get; set; } = new List<int>();
            public ICollection<int>? Games { get; set; } = new List<int>();
            public ICollection<int>? Characters { get; set; } = new List<int>();
            public ICollection<int>? Companies { get; set; } = new List<int>();
        }
    }
}