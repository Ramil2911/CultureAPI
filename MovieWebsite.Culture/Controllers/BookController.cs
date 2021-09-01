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
    [Route("culture/books")]
    public class BookController : Controller
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FetchBook(int id)
        {
            await using var db = new CultureContext();
            var book = await db.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (book is null) return NotFound("Requested book not found");
            return Json(book);
        }

        /// <summary>
        /// Adds Book to database
        /// </summary>
        /// <returns>Returns id of added Book if success</returns>
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> AddBook([FromBody] BookRequestBody body)
        {
            await using var db = new CultureContext();

            var errorBuilder = new StringBuilder("");
            if (body.Name is null) errorBuilder.Append("Name data not found;\n");
            if (body.OriginalName is null) errorBuilder.Append("OriginalName data not found;\n");
            if (!body.PosterId.HasValue) errorBuilder.Append("PosterId data not found;\n");
            if (body.Description is null) errorBuilder.Append("Description data not found;\n");
            if (body.Authors is null) errorBuilder.Append("Authors data not found;\n");
            if (body.Characters is null) errorBuilder.Append("Characters data not found;\n");
            if (errorBuilder.Length != 0) return BadRequest(errorBuilder.ToString());

            var book = new Book
            {
                Name = body.Name!,
                OriginalName = body.OriginalName!,
                PosterId = body.PosterId!.Value,
                Description = body.Description!,
                Authors = await db.Persons.Where(x=>body.Authors!.Contains(x.Id)).ToArrayAsync(),
                Characters = await db.Characters.Where(x=>body.Characters!.Contains(x.Id)).ToArrayAsync(),
            };
            if (body.FranchiseId.HasValue)
                book.Franchise = await db.Franchises.FirstOrDefaultAsync(x => body.FranchiseId!.Value == x.Id);
            else
                book.Franchise = null;
            await db.Books.AddAsync(book);
            
            await db.SaveChangesAsync();
            return Ok(new {id=book.Id});
        }

        /// <summary>
        /// Updates Book in database
        /// </summary>
        /// <returns>Returns id of added Book if success, set null if you dont want to update</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookRequestBody body)
        {
            //this function should not be run very often, so efficiency is not so important
            await using var db = new CultureContext();
            var book = await db.Books
                .FirstOrDefaultAsync(x => x.Id == id);
            if (book is null) return BadRequest("Book not found");
            
            if (body.Name is not null)
                book.Name = body.Name;
            if (body.OriginalName is not null)
                book.OriginalName = body.OriginalName;
            if (body.PosterId.HasValue)
                book.PosterId = body.PosterId.Value;
            if (body.Description is not null)
                book.Description = body.Description;
            if (body.Authors is not null && body.Authors.Count > 0)
                book.Authors = await db.Persons.Where(x => body.Authors.Contains(x.Id)).ToArrayAsync();
            if (body.Characters is not null && body.Characters.Count > 0)
                book.Characters = await db.Characters.Where(x => body.Characters.Contains(x.Id)).ToArrayAsync();
            if (body.FranchiseId.HasValue)
                book.Franchise = await db.Franchises.FirstOrDefaultAsync(x => x.Id == body.FranchiseId);

            await db.SaveChangesAsync();
            return Ok(new {id=book.Id});
        }
        
        /// <summary>
        /// Deletes Book from database
        /// </summary>
        /// <param name="id">Id of Book to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete()]
        public async Task<IActionResult> RemoveBook(int id)
        {
            await using var db = new CultureContext();
            var book = await db.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (book is null) return NotFound();
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("popular")]
        public async Task<IActionResult> FetchBooks(uint lenght, uint skip)
        {
            await using var db = new CultureContext();
            var books = await db.Books.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(books);
        }

        public class BookRequestBody
        {
            public Guid? PosterId { get; set; }
            public string? Description { get; set; }
            public string? Name { get; set; }
            public string? OriginalName { get; set; }
            public int? FranchiseId { get; set; }
            public ICollection<int>? Authors { get; set; } = new List<int>();
            public ICollection<int>? Characters { get; set; } = new List<int>();
        }
    }
}