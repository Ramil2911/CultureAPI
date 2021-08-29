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
    /// <summary>
    /// Contains endpoints for drafts API
    /// </summary>
    [Route("drafts")]
    public class DraftsController : Controller
    {
        
        /// <summary>
        /// An endpoint to add new draft (name only)
        /// </summary>
        /// <param name="title">Name of draft</param>
        /// <returns>Object with guid of new draft</returns>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="400">Drafts limit reached</response>
        /// <response code="200">Draft successfully added</response>
        /// <returns>Json with object which contains guid</returns>
        [ProducesResponseType(typeof(GuidTitlePair), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [HttpPut("new/{title}")]
        [Authorize]
        public async Task<IActionResult> AddDraft(string title)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            if (await db.Drafts.Where(x => x.AuthorId == userId).CountAsync() >= 10)
                return BadRequest("Drafts limit reached");
            
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
            return Json(new GuidTitlePair {Guid=draft.Guid, Title = draft.Title});
        }
        
        /// <summary>
        /// An endpoint to update draft
        /// </summary>
        /// <param name="guid">Guid of draft to update</param>
        /// <param name="body">Body of updated draft</param>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="403">You have no access to that draft</response>
        /// <response code="404">Draft not found</response>
        /// <response code="200">Draft successfully added</response>
        /// <returns>Nothing</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(400)]
        [HttpPut("{guid:Guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateDraft(Guid guid, [FromBody] DraftRequestBody body)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            var draft = await db.Drafts.FirstOrDefaultAsync(x => x.Guid == guid);
            if (draft is null) return NotFound("Draft not found");
            if (draft.AuthorId != userId) return Forbid("You have no access to that draft");
            
            if (body.BodyRaw is not null)
                draft.BodyRaw = body.BodyRaw;
            if (body.Title is not null)
                draft.Title = body.Title;
            if (body.Note is not null)
                draft.Note = body.Note;
            
            await db.SaveChangesAsync();

            return Ok();
        }
        
        /// <summary>
        /// An endpoint to delete draft
        /// </summary>
        /// <param name="guid">Guid of draft to delete</param>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="403">You have no access to that draft</response>
        /// <response code="404">Draft not found</response>
        /// <response code="200">Draft successfully deleted</response>
        /// <returns>Nothing</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpDelete("{guid:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteDraft(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            var draft = await db.Drafts.FirstOrDefaultAsync(x => x.Guid == guid);
            if (draft is null) return NotFound("Draft not found");
            if (draft.AuthorId != userId) return Forbid();
            db.Drafts.Remove(draft);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        /// <summary>
        /// An endpoint to get user's drafts
        /// </summary>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="200">Success</response>
        /// <returns>Array of draft guids</returns>
        [ProducesResponseType(200, Type = typeof(GuidTitlePair[]))]
        [ProducesResponseType(401)]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> FetchDrafts()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            var draftGuids = await db.Drafts.AsNoTracking()
                .Where(x => x.AuthorId == userId)
                .Select(x=> new GuidTitlePair {Guid = x.Guid, Title = x.Title}).ToArrayAsync();
            return Json(draftGuids);
        }
        
        /// <summary>
        /// An endpoint to get draft
        /// </summary>
        /// <param name="guid">Guid of draft to get</param>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="403">You have no access to that draft</response>
        /// <response code="404">Draft not found</response>
        /// <response code="200">Success</response>
        /// <returns>Json of draft</returns>
        [ProducesResponseType(200, Type = typeof(ShortPost))]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [HttpGet("{guid:Guid}")]
        [Authorize]
        public async Task<IActionResult> FetchDraft(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var db = new DraftsContext();
            var draft = await db.Drafts.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == guid);
            if (draft is null) return NotFound("Draft not found");
            if (draft.AuthorId != userId) return Forbid();
            return Json(draft.Short);
        }

        /// <summary>
        /// Body of draft endpoints
        /// </summary>
        public class DraftRequestBody
        {
            /// <summary>
            /// Raw json of post for Editor.js
            /// </summary>
            public string? BodyRaw { get; set; }
            /// <summary>
            /// Html body of post
            /// </summary>
            [Obsolete("Not used anymore due to vulnerability")]
            public string? BodyHtml { get; set; }
            /// <summary>
            /// Title of post
            /// </summary>
            public string? Title { get; set; }
            /// <summary>
            /// Admin's note.
            /// </summary>
            public string? Note { get; set; }
        }

        
        /// <summary>
        /// Container for guid+title
        /// </summary>
        public class GuidTitlePair
        {
            /// <summary>
            /// Value
            /// </summary>
            public Guid Guid { get; set; }
            /// <summary>
            /// Title
            /// </summary>
            public string Title { get; set; }
        }
    }
}