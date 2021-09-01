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
    [Route("culture/movies")]
    public class MovieController : Controller
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FetchMovie(int id)
        {
            await using var db = new CultureContext();
            var movie = await db.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (movie is null) return NotFound("Requested movie not found");
            return Json(movie);
        }

        /// <summary>
        /// Adds Movie to database
        /// </summary>
        /// <returns>Returns id of added Movie if success</returns>
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> AddMovie([FromBody] MovieRequestBody body)
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

            var movie = new Movie
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
                movie.Franchise = await db.Franchises.FirstOrDefaultAsync(x => body.FranchiseId!.Value == x.Id);
            else
                movie.Franchise = null;
            await db.Movies.AddAsync(movie);
            
            await db.SaveChangesAsync();
            return Ok(new {id=movie.Id});
        }

        /// <summary>
        /// Updates Movie in database
        /// </summary>
        /// <returns>Returns id of added Movie if success, set null if you dont want to update</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieRequestBody body)
        {
            //this function should not be run often, so efficiency is not important
            await using var db = new CultureContext();
            var movie = await db.Movies
                .FirstOrDefaultAsync(x => x.Id == id);
            if (movie is null) return BadRequest("Movie not found");
            
            if (body.OriginalName is not null)
                movie.OriginalName = body.OriginalName;
            if (body.Name is not null)
                movie.Name = body.Name;
            if (body.Description is not null)
                movie.Description = body.Description;
            if (body.Directors is not null && body.Directors.Count > 0)
                movie.Directors = await db.Persons.Where(x => body.Directors.Contains(x.Id)).ToArrayAsync();
            if (body.Characters is not null && body.Characters.Count > 0)
                movie.Characters = await db.Characters.Where(x => body.Characters.Contains(x.Id)).ToArrayAsync();
            if (body.Actors is not null && body.Actors.Count > 0)
                movie.Actors = await db.Persons.Where(x => body.Actors.Contains(x.Id)).ToArrayAsync();
            if (body.Genres is not null && body.Genres.Count > 0)
                movie.Genres = body.Genres;
            if (body.FranchiseId.HasValue)
                movie.Franchise = await db.Franchises.FirstOrDefaultAsync(x => x.Id == body.FranchiseId);
            

            await db.SaveChangesAsync();
            return Ok(new {id=movie.Id});
        }
        
        /// <summary>
        /// Deletes Movie from database
        /// </summary>
        /// <param name="id">Id of Movie to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete()]
        public async Task<IActionResult> RemoveMovie(int id)
        {
            await using var db = new CultureContext();
            var movie = await db.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (movie is null) return NotFound();
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("popular")]
        public async Task<IActionResult> FetchMovies(uint lenght, uint skip)
        {
            await using var db = new CultureContext();
            var movies = await db.Movies.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(movies);
        }

        public class MovieRequestBody
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