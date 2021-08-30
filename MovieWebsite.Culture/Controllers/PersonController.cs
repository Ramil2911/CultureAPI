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
    [Route("culture/persons")]
    public class PersonController : Controller
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FetchPerson(int id)
        {
            await using var db = new CultureContext();
            var person = await db.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (person is null) return NotFound("Requested person not found");
            return Json(person);
        }

        /// <summary>
        /// Adds Person to database
        /// </summary>
        /// <returns>Returns id of added Person if success</returns>
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> AddPerson([FromBody] PersonRequestBody body)
        {
            await using var db = new CultureContext();

            var errorBuilder = new StringBuilder("");
            if (body.FullName is null) errorBuilder.Append("FullName data not found;\n");
            if (body.OriginalFullName is null) errorBuilder.Append("FullOriginalName data not found;\n");
            if (body.Characters is null) errorBuilder.Append("Characters data not found;\n");
            if (body.PosterId is null) errorBuilder.Append("Poster data not found;\n");
            if (body.Description is null) errorBuilder.Append("Description data not found;\n");
            if (body.MoviesAsActor is null) errorBuilder.Append("MoviesAsActor data not found;\n");
            if (body.MoviesAsDirector is null) errorBuilder.Append("MoviesAsDirector data not found;\n");
            if (body.SerialsAsActor is null) errorBuilder.Append("SerialsAsActor data not found;\n");
            if (body.SerialsAsDirector is null) errorBuilder.Append("SerialsAsDirector data not found;\n");
            if (errorBuilder.Length != 0) return BadRequest(errorBuilder.ToString());

            var person = new Person
            {
                FullName = body.FullName!,
                OriginalFullName = body.OriginalFullName!,
                PosterId = body.PosterId!.Value,
                Description = body.Description!,
                Characters = await db.Characters.Where(x=>body.Characters!.Contains(x.Id)).ToArrayAsync(),
                MoviesAsActor = await db.Movies.Where(x=>body.MoviesAsActor!.Contains(x.Id)).ToArrayAsync(),
                MoviesAsDirector = await db.Movies.Where(x=>body.MoviesAsDirector!.Contains(x.Id)).ToArrayAsync(),
                SerialsAsActor = await db.Serials.Where(x=>body.SerialsAsActor!.Contains(x.Id)).ToArrayAsync(),
                SerialsAsDirector = await db.Serials.Where(x=>body.SerialsAsDirector!.Contains(x.Id)).ToArrayAsync(),
            };
            await db.Persons.AddAsync(person);
            
            await db.SaveChangesAsync();
            return Ok(new {id=person.Id});
        }

        /// <summary>
        /// Updates Person in database
        /// </summary>
        /// <returns>Returns id of added Person if success, set null if you dont want to update</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] PersonRequestBody body)
        {
            //this function should not be run often, so efficiency is not important
            await using var db = new CultureContext();
            var person = await db.Persons
                .FirstOrDefaultAsync(x => x.Id == id);
            if (person is null) return BadRequest("Person not found");
            
            if (body.FullName is not null)
                person.FullName = body.FullName;
            if (body.OriginalFullName is not null)
                person.OriginalFullName = body.OriginalFullName;
            if (body.Description is not null)
                person.Description = body.Description;
            if (body.MoviesAsActor is not null && body.MoviesAsActor.Count > 0)
                person.MoviesAsActor = await db.Movies.Where(x => body.MoviesAsActor.Contains(x.Id)).ToArrayAsync();
            if (body.MoviesAsDirector is not null && body.MoviesAsDirector.Count > 0)
                person.MoviesAsDirector = await db.Movies.Where(x => body.MoviesAsDirector.Contains(x.Id)).ToArrayAsync();
            if (body.Characters is not null && body.Characters.Count > 0)
                person.Characters = await db.Characters.Where(x => body.Characters.Contains(x.Id)).ToArrayAsync();
            if (body.SerialsAsActor is not null && body.SerialsAsActor.Count > 0)
                person.SerialsAsActor = await db.Serials.Where(x => body.SerialsAsActor.Contains(x.Id)).ToArrayAsync();
            if (body.SerialsAsDirector is not null && body.SerialsAsDirector.Count > 0)
                person.SerialsAsDirector = await db.Serials.Where(x => body.SerialsAsDirector.Contains(x.Id)).ToArrayAsync();


            await db.SaveChangesAsync();
            return Ok(new {id=person.Id});
        }
        
        /// <summary>
        /// Deletes Person from database
        /// </summary>
        /// <param name="id">Id of Person to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete()]
        public async Task<IActionResult> RemovePerson(int id)
        {
            await using var db = new CultureContext();
            var person = await db.Persons.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (person is null) return NotFound();
            db.Persons.Remove(person);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("popular")]
        public async Task<IActionResult> FetchPersons(uint lenght, uint skip)
        {
            await using var db = new CultureContext();
            var persons = await db.Persons.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(persons);
        }

        public class PersonRequestBody
        {
            public Guid? PosterId { get; set; }
            public string? Description { get; set; }
            public string? FullName { get; set; }
            public string? OriginalFullName { get; set; }
            public ICollection<int>? Characters { get; set; } = new List<int>();
            public ICollection<int>? MoviesAsDirector { get; set; } = new List<int>();
            public ICollection<int>? MoviesAsActor { get; set; } = new List<int>();
            public ICollection<int>? SerialsAsDirector { get; set; } = new List<int>();
            public ICollection<int>? SerialsAsActor { get; set; } = new List<int>();
        }
    }
}