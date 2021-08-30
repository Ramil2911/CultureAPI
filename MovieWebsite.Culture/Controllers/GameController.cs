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
[Route("culture/games")]
    public class GameController : Controller
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FetchGame(int id)
        {
            await using var db = new CultureContext();
            var game = await db.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (game is null) return NotFound("Requested game not found");
            return Json(game);
        }

        /// <summary>
        /// Adds Game to database
        /// </summary>
        /// <returns>Returns id of added Game if success</returns>
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> AddGame([FromBody] GameRequestBody body)
        {
            await using var db = new CultureContext();

            var errorBuilder = new StringBuilder("");
            if (body.Name is null) errorBuilder.Append("Name data not found;\n");
            if (body.Characters is null) errorBuilder.Append("Characters data not found;\n");
            if (body.PosterId is null) errorBuilder.Append("Companies data not found;\n");
            if (body.Description is null) errorBuilder.Append("Games data not found;\n");
            if (body.Developers is null) errorBuilder.Append("Movies data not found;\n");
            if (body.Publishers is null) errorBuilder.Append("Serials data not found;\n");
            if (!body.FranchiseId.HasValue) errorBuilder.Append("Franchise data not found;\n");
            if (errorBuilder.Length != 0) return BadRequest(errorBuilder.ToString());

            var game = new Game
            {
                Name = body.Name!,
                PosterId = body.PosterId!.Value,
                Description = body.Description!,
                Franchise = await db.Franchises.FirstOrDefaultAsync(x=>body.FranchiseId!.Value == x.Id),
                Developers = await db.Companies.Where(x=>body.Developers!.Contains(x.Id)).ToArrayAsync(),
                Characters = await db.Characters.Where(x=>body.Characters!.Contains(x.Id)).ToArrayAsync(),
                Publishers = await db.Companies.Where(x=>body.Publishers!.Contains(x.Id)).ToArrayAsync(),
            };
            await db.Games.AddAsync(game);
            
            await db.SaveChangesAsync();
            return Ok(new {id=game.Id});
        }

        /// <summary>
        /// Updates Game in database
        /// </summary>
        /// <returns>Returns id of added Game if success, set null if you dont want to update</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] GameRequestBody body)
        {
            //this function should not be run often, so efficiency is not important
            await using var db = new CultureContext();
            var game = await db.Games
                .FirstOrDefaultAsync(x => x.Id == id);
            if (game is null) return BadRequest("Game not found");
            
            if (body.Name is not null)
                game.Name = body.Name;
            if (body.Description is not null)
                game.Description = body.Description;
            if (body.Developers is not null && body.Developers.Count > 0)
                game.Developers = await db.Companies.Where(x => body.Developers.Contains(x.Id)).ToArrayAsync();
            if (body.Characters is not null && body.Characters.Count > 0)
                game.Characters = await db.Characters.Where(x => body.Characters.Contains(x.Id)).ToArrayAsync();
            if (body.Publishers is not null && body.Publishers.Count > 0)
                game.Publishers = await db.Companies.Where(x => body.Publishers.Contains(x.Id)).ToArrayAsync();
            if (body.FranchiseId.HasValue)
                game.Franchise = await db.Franchises.FirstOrDefaultAsync(x => x.Id == body.FranchiseId);

            await db.SaveChangesAsync();
            return Ok(new {id=game.Id});
        }
        
        /// <summary>
        /// Deletes Game from database
        /// </summary>
        /// <param name="id">Id of Game to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete()]
        public async Task<IActionResult> RemoveGame(int id)
        {
            await using var db = new CultureContext();
            var game = await db.Games.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (game is null) return NotFound();
            db.Games.Remove(game);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("popular")]
        public async Task<IActionResult> FetchGames(uint lenght, uint skip)
        {
            await using var db = new CultureContext();
            var games = await db.Companies.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(games);
        }

        public class GameRequestBody
        {
            public Guid? PosterId { get; set; }
            public string? Description { get; set; }
            public string? Name { get; set; }
            [Obsolete("Not used anymore")]
            public string? OriginalName { get; set; }
            public int? FranchiseId { get; set; }
            public ICollection<int>? Developers { get; set; } = new List<int>();
            public ICollection<int>? Publishers { get; set; } = new List<int>();
            public ICollection<int>? Characters { get; set; } = new List<int>();
        }
    }
}