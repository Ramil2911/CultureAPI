using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Characters.Models;
using MovieWebsite.Characters.Models.Databases;
using MovieWebsite.Shared;

namespace MovieWebsite.Characters.Controllers
{
    [Route("Characters")]
    public class CharacterController : Controller
    {
        [HttpGet("FetchCharacter")]
        public async Task<IActionResult> FetchCharacter(int id)
        {
            await using var db = new CharacterContext();
            var character = await db.Characters
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (character is null) return NotFound("Requested Character not found");
            return Json(character);
        }

        /// <summary>
        /// Adds Character to database
        /// </summary>
        /// <param name="name">Character name in russian</param>
        /// <param name="originalName">Character name in its original language</param>
        /// <param name="posterId">Id of Character poster on image server</param>
        /// <param name="description">Description of Character in Russian</param>
        /// <param name="directorIds">Ids of Character directors</param>
        /// <param name="actorIds">Ids of Character actors</param>
        /// <param name="characterIds">Ids of Character characters</param>
        /// <param name="genres">Genre of Character see <see cref="Genre"/></param>
        /// <returns>Returns id of added Character if success</returns>
        [Authorize()]
        [HttpPost("AddCharacter")]
        public async Task<IActionResult> AddCharacter(string name, string originalName, int posterId, string description,
            [FromQuery] HashSet<int> persons, [FromQuery] HashSet<int> movies)
        {
            await using var db = new CharacterContext();
            var character = new Character
            {
                FullName = name,
                OriginalFullName = originalName,
                PosterId = posterId,
                Description = description,
                Persons = persons,
                Movies = movies,
            };
            await db.Characters.AddAsync(character);
            
            await db.SaveChangesAsync();
            return Ok(new {id=character.Id});
        }

        /// <summary>
        /// Updates Character in database
        /// </summary>
        /// <param name="id">Id of Character to update</param>
        /// <param name="name">Character name in russian, set null if you dont want to update</param>
        /// <param name="originalName">Character name in its original language, set null if you dont want to update</param>
        /// <param name="posterId">Id of Character poster on image server, set null if you dont want to update</param>
        /// <param name="description">Description of Character in Russian, set null if you dont want to update</param>
        /// <param name="directorIds">Ids of Character directors, set null if you dont want to update</param>
        /// <param name="actorIds">Ids of Character actors, set null if you dont want to update</param>
        /// <param name="characterIds">Ids of Character characters, set null if you dont want to update</param>
        /// <param name="genres">Genre of Character see, set null if you dont want to update <see cref="Genre"/></param>
        /// <returns>Returns id of added Character if success, set null if you dont want to update</returns>
        [Authorize]
        [HttpPost("UpdateCharacter")]
        public async Task<IActionResult> UpdateCharacter(int id, string? name, string? originalName, int? posterId, string? description,
            [FromQuery] HashSet<int>? persons, [FromQuery] HashSet<int>? movies)
        {
            //this function should not be run very often, so efficiency is not so important
            await using var db = new CharacterContext();
            var character = await db.Characters
                .FirstOrDefaultAsync(x => x.Id == id);
            if (character is null) return BadRequest("Character not found");
            if (originalName is not null)
                character.OriginalFullName = originalName;
            if (posterId > 0)
                character.PosterId = posterId.Value;
            if (description is not null)
                character.Description = description;
            if (name is not null)
                character.FullName = name;
            if (persons is not null && persons.Count > 0)
                character.Persons = persons;
            if (movies is not null && movies.Count > 0)
                character.Movies = movies;
            
            await db.SaveChangesAsync();
            return Ok(new {id=character.Id});
        }
        
        /// <summary>
        /// Deletes Character from database
        /// </summary>
        /// <param name="id">Id of Character to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete("RemoveCharacter")]
        public async Task<IActionResult> RemoveCharacter(int id)
        {
            await using var db = new CharacterContext();
            var character = await db.Characters.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (character is null) return NotFound();
            db.Characters.Remove(character);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("FetchCharacters")]
        public async Task<IActionResult> FetchCharacters(uint lenght, uint skip)
        {
            await using var db = new CharacterContext();
            var characters = await db.Characters.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(characters);
        }
    }
}