using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Blogs.Models;
using MovieWebsite.Blogs.Models.Databases;
using MovieWebsite.Shared;

namespace MovieWebsite.Blogs.Controllers
{
    [Route("drafts")]
    public class DraftsController : Controller
    {
        [HttpPut("new/{title}")]
        [Authorize]
        public async Task<IActionResult> AddDraft(string title)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            var draft = new Post
            {
                AuthorId = userId.Value,
                BodyHtml = "",
                BodyRaw = "",
                Title = title,
                Note = ""
            };
            await db.Drafts.AddAsync(draft);
            await db.SaveChangesAsync();
            return Json(new {guid=draft.Guid});
        }
        
        [HttpPut("{guid:Guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateDraft(Guid guid, [FromBody] UpdatePostBody body)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            var draft = await db.Drafts.FirstOrDefaultAsync(x => x.Guid == guid);

            if (body.BodyHtml is not null)
                draft.BodyHtml = body.BodyHtml;
            if (body.BodyRaw is not null)
                draft.BodyRaw = body.BodyRaw;
            if (body.Title is not null)
                draft.Title = body.Title;
            if (body.Note is not null)
                draft.Note = body.Note;
            
            await db.SaveChangesAsync();

            return Ok();
        }
        
        [HttpDelete("{guid:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteDraft(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            var draft = await db.Drafts.FirstOrDefaultAsync(x => x.Guid == guid);
            if (draft.AuthorId != userId) return Forbid();
            db.Drafts.Remove(draft);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FetchDrafts()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            var draftGuids = await db.Drafts.AsNoTracking().Where(x => x.AuthorId == userId).Select(x=> new {guid=x.Guid, title=x.Title}).ToArrayAsync();
            return Json(draftGuids);
        }
        
        [HttpGet("{guid:Guid}")]
        [Authorize]
        public async Task<IActionResult> FetchDraft(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            var draft = await db.Drafts.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == guid);
            if (draft.AuthorId != userId) return Forbid();
            return Json(draft);
        }

        public class UpdatePostBody
        {
            public string? BodyRaw { get; set; }
            public string? BodyHtml { get; set; }
            public string? Title { get; set; }
            public string? Note { get; set; }
        }
    }
}