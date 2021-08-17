using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Movies.Models;
using MovieWebsite.Movies.Models.Databases;
using MovieWebsite.Shared;

namespace MovieWebsite.Movies.Controllers
{
    [Route("movies")]
    public class MovieController : Controller
    {
                [HttpGet("FetchMovie")]
        public async Task<IActionResult> FetchMovie(int id)
        {
            await using var db = new MovieContext();
            var movie = await db.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (movie is null) return NotFound("Requested movie not found");
            return Json(movie);
        }

        /// <summary>
        /// Adds movie to database
        /// </summary>
        /// <param name="name">Movie name in russian</param>
        /// <param name="originalName">Movie name in its original language</param>
        /// <param name="posterId">Id of movie poster on image server</param>
        /// <param name="description">Description of movie in Russian</param>
        /// <param name="directorIds">Ids of movie directors</param>
        /// <param name="actorIds">Ids of movie actors</param>
        /// <param name="characterIds">Ids of movie characters</param>
        /// <param name="genres">Genre of movie see <see cref="Genre"/></param>
        /// <returns>Returns id of added movie if success</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("AddMovie")]
        public async Task<IActionResult> AddMovie(string name, string originalName, int posterId, string description,
            [FromQuery] HashSet<int> directorIds, [FromQuery] HashSet<int> actorIds,
            [FromQuery] HashSet<int> characterIds, [FromQuery] HashSet<Genre> genres)
        {
            await using var db = new MovieContext();
            var movie = new Movie
            {
                Name = name,
                OriginalName = originalName,
                PosterId = posterId,
                Description = description,
                Genres = genres,
                DirectorIds = directorIds,
                ActorIds = actorIds,
                CharacterIds = characterIds
            };
            db.Movies.Add(movie);
            
            await db.SaveChangesAsync();
            return Ok(new {id=movie.Id});
        }

        /// <summary>
        /// Updates movie in database
        /// </summary>
        /// <param name="id">Id of movie to update</param>
        /// <param name="name">Movie name in russian, set null if you dont want to update</param>
        /// <param name="originalName">Movie name in its original language, set null if you dont want to update</param>
        /// <param name="posterId">Id of movie poster on image server, set null if you dont want to update</param>
        /// <param name="description">Description of movie in Russian, set null if you dont want to update</param>
        /// <param name="directorIds">Ids of movie directors, set null if you dont want to update</param>
        /// <param name="actorIds">Ids of movie actors, set null if you dont want to update</param>
        /// <param name="characterIds">Ids of movie characters, set null if you dont want to update</param>
        /// <param name="genres">Genre of movie see, set null if you dont want to update <see cref="Genre"/></param>
        /// <returns>Returns id of added movie if success, set null if you dont want to update</returns>
        [Authorize]
        [HttpPost("UpdateMovie")]
        public async Task<IActionResult> UpdateMovie(int id, string? name, string? originalName, int? posterId, string? description,
            [FromQuery] HashSet<int>? directorIds, [FromQuery] HashSet<int>? actorIds,
            [FromQuery] HashSet<int>? characterIds, [FromQuery] HashSet<Genre>? genres)
        {
            //this function should not be run very often, so efficiency is not so important
            await using var db = new MovieContext();
            var movie = await db.Movies
                .FirstOrDefaultAsync(x => x.Id == id);
            if (movie is null) return BadRequest("Movie not found");
            if (originalName is not null)
                movie.OriginalName = originalName;
            if (posterId > 0)
                movie.PosterId = posterId.Value;
            if (description is not null)
                movie.Description = description;
            if (name is not null)
                movie.Name = name;
            if (genres is not null && genres.Count > 0)
                movie.Genres = genres;
            if (directorIds is not null && directorIds.Count > 0)
                movie.DirectorIds = directorIds;
            if (actorIds is not null && actorIds.Count > 0)
                movie.ActorIds = actorIds;
            if (characterIds is not null && characterIds.Count > 0)
                movie.CharacterIds = characterIds;
            
            await db.SaveChangesAsync();
            return Ok(new {id=movie.Id});
        }
        
        /// <summary>
        /// Deletes movie from database
        /// </summary>
        /// <param name="id">Id of movie to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("RemoveMovie")]
        public async Task<IActionResult> RemoveMovie(int id)
        {
            await using var db = new MovieContext();
            var movie = await db.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (movie is null) return NotFound();
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("FetchMovies")]
        public async Task<IActionResult> FetchMovies(uint lenght, uint skip)
        {
            await using var db = new MovieContext();
            var movies = await db.Movies.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(movies);
        }
    }
}