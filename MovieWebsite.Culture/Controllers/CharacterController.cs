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

namespace MovieWebsite.Movies.Controllers
{
    [Route("culture/characters")]
    public class CharacterController : Controller
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FetchCharacter(int id)
        {
            await using var db = new CultureContext();
            var character = await db.Characters
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (character is null) return NotFound("Requested Character not found");
            return Json(character);
        }

        /// <summary>
        /// Adds Character to database
        /// </summary>
        /// <returns>Returns id of added Character if success</returns>
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> AddCharacter([FromBody] CharacterRequestBody body)
        {
            await using var db = new CultureContext();

            var errorBuilder = new StringBuilder("");
            if (body.Name is null) errorBuilder.Append("Name data not found;\n");
            if (body.OriginalName is null) errorBuilder.Append("OriginalName data not found;\n");
            if (!body.PosterId.HasValue) errorBuilder.Append("PosterId data not found;\n");
            if (body.Description is null) errorBuilder.Append("Description data not found;\n");
            if (!body.FranchiseId.HasValue) errorBuilder.Append("Franchise data not found;\n");
            if (body.Actors is null) errorBuilder.Append("Actors data not found;\n");
            if (body.Books is null) errorBuilder.Append("Books data not found;\n");
            if (body.Games is null) errorBuilder.Append("Games data not found;\n");
            if (body.Movies is null) errorBuilder.Append("Movies data not found;\n");
            if (body.Serials is null) errorBuilder.Append("Serials data not found;\n");
            if (errorBuilder.Length != 0) return BadRequest(errorBuilder.ToString());

            var character = new Character
            {
                FullName = body.Name,
                OriginalFullName = body.OriginalName,
                PosterId = body.PosterId.Value,
                Description = body.Description,
                Franchise = await db.Franchises.FirstOrDefaultAsync(x=>body.FranchiseId.Value == x.Id),
                Actors = await db.Persons.Where(x=>body.Actors.Contains(x.Id)).ToArrayAsync(),
                Books = await db.Books.Where(x=>body.Books.Contains(x.Id)).ToArrayAsync(),
                Games = await db.Games.Where(x=>body.Games.Contains(x.Id)).ToArrayAsync(),
                Movies = await db.Movies.Where(x=>body.Movies.Contains(x.Id)).ToArrayAsync(),
                Serials = await db.Serials.Where(x=>body.Serials.Contains(x.Id)).ToArrayAsync(),
            };
            if (body.FranchiseId.HasValue)
                character.Franchise = await db.Franchises.FirstOrDefaultAsync(x => body.FranchiseId!.Value == x.Id);
            else
                character.Franchise = null;
            await db.Characters.AddAsync(character);
            
            await db.SaveChangesAsync();
            return Ok(new {id=character.Id});
        }

        /// <summary>
        /// Updates Character in database
        /// </summary>
        /// <returns>Returns id of added Character if success, set null if you dont want to update</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateCharacter(int id, [FromBody] CharacterRequestBody body)
        {
            //this function should not be run very often, so efficiency is not so important
            await using var db = new CultureContext();
            var character = await db.Characters
                .FirstOrDefaultAsync(x => x.Id == id);
            if (character is null) return BadRequest("Character not found");
            
            if (body.Name is not null)
                character.FullName = body.Name;
            if (body.OriginalName is not null)
                character.OriginalFullName = body.OriginalName;
            if (body.PosterId.HasValue)
                character.PosterId = body.PosterId.Value;
            if (body.Description is not null)
                character.Description = body.Description;
            if (body.Actors is not null && body.Actors.Count > 0)
                character.Actors = await db.Persons.Where(x => body.Actors.Contains(x.Id)).ToArrayAsync();
            if (body.Books is not null && body.Books.Count > 0)
                character.Books = await db.Books.Where(x => body.Books.Contains(x.Id)).ToArrayAsync();
            if (body.Games is not null && body.Games.Count > 0)
                character.Games = await db.Games.Where(x => body.Games.Contains(x.Id)).ToArrayAsync();
            if (body.Movies is not null && body.Movies.Count > 0)
                character.Movies = await db.Movies.Where(x => body.Movies.Contains(x.Id)).ToArrayAsync();
            if (body.Serials is not null && body.Serials.Count > 0)
                character.Serials = await db.Serials.Where(x => body.Actors.Contains(x.Id)).ToArrayAsync();
            if (body.FranchiseId.HasValue)
                character.Franchise = await db.Franchises.FirstOrDefaultAsync(x => x.Id == body.FranchiseId);

            await db.SaveChangesAsync();
            return Ok(new {id=character.Id});
        }
        
        /// <summary>
        /// Deletes Character from database
        /// </summary>
        /// <param name="id">Id of Character to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete()]
        public async Task<IActionResult> RemoveCharacter(int id)
        {
            await using var db = new CultureContext();
            var character = await db.Characters.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (character is null) return NotFound();
            db.Characters.Remove(character);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("popular")]
        public async Task<IActionResult> FetchCharacters(uint lenght, uint skip)
        {
            await using var db = new CultureContext();
            var characters = await db.Characters.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(characters);
        }

        public class CharacterRequestBody
        {
            public Guid? PosterId { get; set; }
            public string? Description { get; set; }
            public string? Name { get; set; }
            public string? OriginalName { get; set; }
            public int? FranchiseId { get; set; }
            public ICollection<int>? Actors { get; set; }
            public ICollection<int>? Books { get; set; }
            public ICollection<int>? Games { get; set; }
            public ICollection<int>? Movies { get; set; }
            public ICollection<int>? Serials { get; set; }
        }
    }
}