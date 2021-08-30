using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Movies.Models;
using MovieWebsite.Movies.Models.Databases;
using MovieWebsite.Shared;

/*namespace MovieWebsite.Movies.Controllers
{
    public class PersonController : Controller
    {
        [HttpGet("FetchPerson")]
        public async Task<IActionResult> FetchPerson(int id)
        {
            await using var db = new PersonContext();
            var person = await db.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (person is null) return NotFound("Requested Person not found");
            return Json(person);
        }

        /// <summary>
        /// Adds Person to database
        /// </summary>
        /// <param name="name">Person name in russian</param>
        /// <param name="originalName">Person name in its original language</param>
        /// <param name="posterId">Id of Person poster on image server</param>
        /// <param name="description">Description of Person in Russian</param>
        /// <param name="directorIds">Ids of Person directors</param>
        /// <param name="actorIds">Ids of Person actors</param>
        /// <param name="PersonIds">Ids of Person Persons</param>
        /// <param name="genres">Genre of Person see <see cref="Genre"/></param>
        /// <returns>Returns id of added Person if success</returns>
        [Authorize()]
        [HttpPost("AddPerson")]
        public async Task<IActionResult> AddPerson(string name, string originalName, Guid?PosterId, string description,
            [FromQuery] HashSet<int> characters, [FromQuery] HashSet<int> movies)
        {
            await using var db = new PersonContext();
            var person = new Person
            {
                FullName = name,
                OriginalFullName = originalName,
                PosterId = posterId,
                Description = description,
                Characters = characters,
                Movies = movies,
            };
            await db.Persons.AddAsync(person);
            
            await db.SaveChangesAsync();
            return Ok(new {id=person.Id});
        }

        /// <summary>
        /// Updates Person in database
        /// </summary>
        /// <param name="id">Id of Person to update</param>
        /// <param name="name">Person name in russian, set null if you dont want to update</param>
        /// <param name="originalName">Person name in its original language, set null if you dont want to update</param>
        /// <param name="posterId">Id of Person poster on image server, set null if you dont want to update</param>
        /// <param name="description">Description of Person in Russian, set null if you dont want to update</param>
        /// <param name="directorIds">Ids of Person directors, set null if you dont want to update</param>
        /// <param name="actorIds">Ids of Person actors, set null if you dont want to update</param>
        /// <param name="PersonIds">Ids of Person Persons, set null if you dont want to update</param>
        /// <param name="genres">Genre of Person see, set null if you dont want to update <see cref="Genre"/></param>
        /// <returns>Returns id of added Person if success, set null if you dont want to update</returns>
        [Authorize]
        [HttpPost("UpdatePerson")]
        public async Task<IActionResult> UpdatePerson(int id, string? name, string? originalName, Guid? PosterId, string? description,
            [FromQuery] HashSet<int>? characters, [FromQuery] HashSet<int>? movies)
        {
            //this function should not be run very often, so efficiency is not so important
            await using var db = new PersonContext();
            var person = await db.Persons
                .FirstOrDefaultAsync(x => x.Id == id);
            if (person is null) return BadRequest("Person not found");
            if (originalName is not null)
                person.OriginalFullName = originalName;
            if (posterId > 0)
                person.PosterId = posterId.Value;
            if (description is not null)
                person.Description = description;
            if (name is not null)
                person.FullName = name;
            if (characters is not null && characters.Count > 0)
                person.Characters = characters;
            if (movies is not null && movies.Count > 0)
                person.Movies = movies;
            
            await db.SaveChangesAsync();
            return Ok(new {id=person.Id});
        }
        
        /// <summary>
        /// Deletes Person from database
        /// </summary>
        /// <param name="id">Id of Person to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("RemovePerson")]
        public async Task<IActionResult> RemovePerson(int id)
        {
            await using var db = new PersonContext();
            var person = await db.Persons.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (person is null) return NotFound();
            db.Persons.Remove(person);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("FetchPersons")]
        public async Task<IActionResult> FetchPersons(uint lenght, uint skip)
        {
            await using var db = new PersonContext();
            var persons = await db.Persons.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(persons);
        }
    }
}*/