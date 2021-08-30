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
    [Route("culture/companys")]
    public class CompanyController : Controller
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FetchCompany(int id)
        {
            await using var db = new CultureContext();
            var company = await db.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (company is null) return NotFound("Requested company not found");
            return Json(company);
        }

        /// <summary>
        /// Adds Company to database
        /// </summary>
        /// <returns>Returns id of added Company if success</returns>
        [Authorize(Roles = "admin")]
        [HttpPut()]
        public async Task<IActionResult> AddCompany([FromBody] CompanyRequestBody body)
        {
            await using var db = new CultureContext();

            var errorBuilder = new StringBuilder("");
            if (body.Name is null) errorBuilder.Append("Name data not found;\n");
            if (!body.PosterId.HasValue) errorBuilder.Append("PosterId data not found;\n");
            if (body.Description is null) errorBuilder.Append("Description data not found;\n");
            if (!body.FranchiseId.HasValue) errorBuilder.Append("Franchise data not found;\n");
            if (body.GamesAsDeveloper is null) errorBuilder.Append("GamesAsDeveloper data not found;\n");
            if (body.GamesAsPublisher is null) errorBuilder.Append("GamesAsPublisher data not found;\n");
            if (body.MoviesAsDeveloper is null) errorBuilder.Append("MoviesAsDeveloper data not found;\n");
            if (body.MoviesAsPublisher is null) errorBuilder.Append("MoviesAsPublisher data not found;\n");
            if (body.SerialsAsDeveloper is null) errorBuilder.Append("SerialsAsDeveloper data not found;\n");
            if (body.SerialsAsPublisher is null) errorBuilder.Append("SerialsAsPublisher data not found;\n");
            if (errorBuilder.Length != 0) return BadRequest(errorBuilder.ToString());

            var company = new Company
            {
                Name = body.Name!,
                PosterId = body.PosterId!.Value,
                Description = body.Description!,
                Franchise = await db.Franchises.FirstOrDefaultAsync(x=>body.FranchiseId!.Value == x.Id),
                GamesAsDeveloper = await db.Games.Where(x=>body.GamesAsDeveloper!.Contains(x.Id)).ToArrayAsync(),
                GamesAsPublisher = await db.Games.Where(x=>body.GamesAsPublisher!.Contains(x.Id)).ToArrayAsync(),
                MoviesAsDeveloper = await db.Movies.Where(x=>body.MoviesAsDeveloper!.Contains(x.Id)).ToArrayAsync(),
                MoviesAsPublisher = await db.Movies.Where(x=>body.MoviesAsPublisher!.Contains(x.Id)).ToArrayAsync(),
                SerialsAsDeveloper = await db.Serials.Where(x=>body.SerialsAsDeveloper!.Contains(x.Id)).ToArrayAsync(),
                SerialsAsPublisher = await db.Serials.Where(x=>body.SerialsAsPublisher!.Contains(x.Id)).ToArrayAsync(),
            };
            await db.Companies.AddAsync(company);
            
            await db.SaveChangesAsync();
            return Ok(new {id=company.Id});
        }

        /// <summary>
        /// Updates Company in database
        /// </summary>
        /// <returns>Returns id of added Company if success, set null if you dont want to update</returns>
        [Authorize(Roles = "admin")]
        [HttpPost("{id:int}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyRequestBody body)
        {
            //this function should not be run often, so efficiency is not important
            await using var db = new CultureContext();
            var company = await db.Companies
                .FirstOrDefaultAsync(x => x.Id == id);
            if (company is null) return BadRequest("Company not found");
            
            if (body.Name is not null)
                company.Name = body.Name;
            if (body.PosterId.HasValue)
                company.PosterId = body.PosterId.Value;
            if (body.Description is not null)
                company.Description = body.Description;
            if (body.GamesAsPublisher is not null && body.GamesAsPublisher.Count > 0)
                company.GamesAsPublisher = await db.Games.Where(x => body.GamesAsPublisher.Contains(x.Id)).ToArrayAsync();
            if (body.GamesAsDeveloper is not null && body.GamesAsDeveloper.Count > 0)
                company.GamesAsDeveloper = await db.Games.Where(x => body.GamesAsDeveloper.Contains(x.Id)).ToArrayAsync();
            if (body.MoviesAsDeveloper is not null && body.MoviesAsDeveloper.Count > 0)
                company.MoviesAsDeveloper = await db.Movies.Where(x => body.MoviesAsDeveloper.Contains(x.Id)).ToArrayAsync();
            if (body.MoviesAsPublisher is not null && body.MoviesAsPublisher.Count > 0)
                company.MoviesAsPublisher = await db.Movies.Where(x => body.MoviesAsPublisher.Contains(x.Id)).ToArrayAsync();
            if (body.SerialsAsDeveloper is not null && body.SerialsAsDeveloper.Count > 0)
                company.SerialsAsDeveloper = await db.Serials.Where(x => body.SerialsAsDeveloper.Contains(x.Id)).ToArrayAsync();
            if (body.SerialsAsPublisher is not null && body.SerialsAsPublisher.Count > 0)
                company.SerialsAsPublisher = await db.Serials.Where(x => body.SerialsAsPublisher.Contains(x.Id)).ToArrayAsync();
            if (body.FranchiseId.HasValue)
                company.Franchise = await db.Franchises.FirstOrDefaultAsync(x => x.Id == body.FranchiseId);

            await db.SaveChangesAsync();
            return Ok(new {id=company.Id});
        }
        
        /// <summary>
        /// Deletes Company from database
        /// </summary>
        /// <param name="id">Id of Company to delete</param>
        /// <returns>Returns Ok if success</returns>
        [Authorize(Roles = "admin")]
        [HttpDelete()]
        public async Task<IActionResult> RemoveCompany(int id)
        {
            await using var db = new CultureContext();
            var company = await db.Companies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (company is null) return NotFound();
            db.Companies.Remove(company);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        //TODO: More settings
        [HttpGet("popular")]
        public async Task<IActionResult> FetchCompanys(uint lenght, uint skip)
        {
            await using var db = new CultureContext();
            var companies = await db.Companies.AsNoTracking()
                .OrderBy(x=>x.Id)
                .Skip((int) skip)
                .Take((int) lenght)
                .ToArrayAsync();
            return Json(companies);
        }

        public class CompanyRequestBody
        {
            public Guid? PosterId { get; set; }
            public string? Description { get; set; }
            public string? Name { get; set; }
            public int? FranchiseId { get; set; }
            public ICollection<int>? MoviesAsPublisher { get; set; } = new List<int>();
            public ICollection<int>? MoviesAsDeveloper { get; set; } = new List<int>();
            public ICollection<int>? SerialsAsPublisher { get; set; } = new List<int>();
            public ICollection<int>? SerialsAsDeveloper { get; set; } = new List<int>();
            public ICollection<int>? GamesAsPublisher { get; set; } = new List<int>();
            public ICollection<int>? GamesAsDeveloper { get; set; } = new List<int>();
        }
    }
}