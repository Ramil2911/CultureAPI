using System;
using System.Collections.Generic;
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
    /// Contains endpoints for blogs API
    /// </summary>
    [ProducesResponseType(typeof(Post),200)]
    [ProducesResponseType(404)]
    [Route("blogs")]
    public class BlogsController : Controller
    {
        /// <summary>
        /// An endpoint to get post by its guid
        /// </summary>
        /// <param name="guid">Guid of post to get</param>
        /// <returns>Json of post</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Post not found</response>
        [ProducesResponseType(typeof(ShortPost),200)]
        [ProducesResponseType(404)]
        [HttpGet("{guid:Guid}")]
        public async Task<IActionResult> FetchPostByGuid(Guid guid)
        {
            await using var db = new BlogsContext();
            var post = await db.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == guid);
            if (post is null) return NotFound("Post not found");
            return Json(post.Short);
        }

        /// <summary>
        /// An endpoint to get posts of user
        /// </summary>
        /// <param name="userId">Id of user to get posts of</param>
        /// <returns>Json of array of posts</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Posts not found</response>
        [ProducesResponseType(typeof(IEnumerable<ShortPost>), 200)]
        [ProducesResponseType(404)]
        [HttpGet("users/{userId:int}")]
        public async Task<IActionResult> FetchUserPosts(int userId)
        {
            await using var db = new BlogsContext();
            var posts = await db.Posts.AsNoTracking().Where(x => x.AuthorId == userId).ToArrayAsync();
            if (posts is null) return NotFound("Posts not found");
            return Json(posts.Select(x=>x.Short));
        }
        
        /// <summary>
        /// An endpoint to get popular posts. Not yet implemented.
        /// </summary>
        /// <param name="amount">Amount of posts to get</param>
        /// <param name="skip">Amount of posts to skip</param>
        /// <returns>Json of array of posts</returns>
        /// <exception cref="NotImplementedException">Not yet implemented.</exception>
        /// <response code="500">Not yet implemented.</response>
        [ProducesResponseType(500)]
        [HttpGet("popular")]
        public async Task<IActionResult> FetchPopularPosts(uint amount, uint skip)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// An endpoint to get latest posts
        /// </summary>
        /// <param name="amount">Amount of posts to get</param>
        /// <param name="skip">Amount of posts to skip</param>
        /// <returns>Json of array of posts</returns>
        /// <response code="200">Success</response>
        [ProducesResponseType(typeof(IEnumerable<ShortPost>), 200)]
        [HttpGet("latest")]
        public async Task<IActionResult> FetchLatestPosts(uint amount, uint skip)
        {
            await using var db = new BlogsContext();
            return Json(await db.Posts.AsNoTracking().OrderBy(x=>x.TimeStamp).Skip((int)skip).Take((int)amount).Select(x=>x.Short).ToArrayAsync());
        }
        
        /// <summary>
        /// An endpoint to add post
        /// </summary>
        /// <param name="guid">Guid of draft to add</param>
        /// <returns>Nothing</returns>
        /// <response code="404">Draft not found</response>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="403">You have no access to that post</response>
        /// <response code="200">Post successfully added</response>
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(200)]
        [Authorize]
        [HttpPut("{guid:Guid}")]
        public async Task<IActionResult> AddPost(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var draftsdb = new DraftsContext();
            var draft = await draftsdb.Drafts.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == guid);
            if (draft is null) return NotFound("Draft not found");
            if (draft.AuthorId != userId) return Forbid("You have no access to that post");

            await using var postsdb = new BlogsContext();
            draft.TimeStamp = DateTime.Now;
            await postsdb.AddAsync(draft);
            var postTask = postsdb.SaveChangesAsync();
            
            draftsdb.Remove(draft); 
            await draftsdb.SaveChangesAsync();
            
            await postTask;
            
            return Ok();
        }
        
        /// <summary>
        /// An endpoint to delete post
        /// </summary>
        /// <param name="guid">Guid of post to delete</param>
        /// <returns>Nothing</returns>
        /// <response code="404">Post not found</response>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="403">You have no access to that post</response>
        /// <response code="200">Post successfully deleted</response>
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(200)]
        [Authorize]
        [HttpDelete("{guid:Guid}")]
        public async Task<IActionResult> DeletePost(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var postsdb = new BlogsContext();
            var post = await postsdb.Posts.FirstOrDefaultAsync(x => x.Guid == guid);
            if (post is null) return NotFound("Post not found");
            if (post.AuthorId != userId) return Forbid("You have no access to that post.");

            postsdb.DeletedPosts.Add(post);
            postsdb.Posts.Remove(post);
            await postsdb.SaveChangesAsync();
            return Ok();
        }
        
        /// <summary>
        /// An endpoint to rankup post
        /// </summary>
        /// <param name="guid">Guid of post to rankup</param>
        /// <returns>Nothing</returns>
        /// <response code="404">Post not found</response>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [Authorize]
        [HttpHead("{guid:Guid}/rankup")]
        public async Task<IActionResult> RankPostUp(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();

            var isModified = false;
            await using var postsdb = new BlogsContext();
            var post = await postsdb.Posts.FirstOrDefaultAsync(x => x.Guid == guid);
            if (post is null) return BadRequest("Post not found");
            if (post.AuthorId == userId) return Forbid();
            if (post.RankDowners.Contains(userId.Value))
            {
                post.RankDowners.Remove(userId.Value);
                isModified = true;
            }
            if (post.RankUppers.Contains(userId.Value))
            {
                if (!isModified) return Ok();
                postsdb.Entry(post).State = EntityState.Modified;
                await postsdb.SaveChangesAsync();
                return Ok();
            }

            post.RankUppers.Add(userId.Value);
            
            postsdb.Entry(post).State = EntityState.Modified;
            await postsdb.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// An endpoint to rankdown post
        /// </summary>
        /// <param name="guid">Guid of post to rankdown</param>
        /// <returns>Nothing</returns>
        /// <response code="404">Post not found</response>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [Authorize]
        [HttpHead("{guid:Guid}/rankdown")]
        public async Task<IActionResult> RankPostDown(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();

            var isModified = false;
            await using var postsdb = new BlogsContext();
            var post = await postsdb.Posts.FirstOrDefaultAsync(x => x.Guid == guid);
            if (post is null) return BadRequest("Post not found");
            if (post.AuthorId == userId) return Forbid();
            if (post.RankUppers.Contains(userId.Value))
            {
                post.RankUppers.Remove(userId.Value);
                isModified = true;
            }
            if (post.RankDowners.Contains(userId.Value))
            {
                if (!isModified) return Ok();
                postsdb.Entry(post).State = EntityState.Modified;
                await postsdb.SaveChangesAsync();
                return Ok();
            }

            post.RankDowners.Add(userId.Value);
            
            postsdb.Entry(post).State = EntityState.Modified;
            await postsdb.SaveChangesAsync();
            return Ok();
        }
        
        /// <summary>
        /// An endpoint to clear user's mark of post
        /// </summary>
        /// <param name="guid">Guid of post to clear mark on</param>
        /// <returns>Nothing</returns>
        /// <response code="404">Post not found</response>
        /// <response code="401">Authorize to use this endpoint</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        [Authorize]
        [HttpHead("{guid:Guid}/rankclean")]
        public async Task<IActionResult> CleanRankPost(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var postsdb = new BlogsContext();
            var post = await postsdb.Posts.FirstOrDefaultAsync(x => x.Guid == guid);
            if (post is null) return BadRequest("Post not found");

            if (post.RankDowners.Contains(userId.Value)) post.RankDowners.Remove(userId.Value);
            if (post.RankUppers.Contains(userId.Value)) post.RankUppers.Remove(userId.Value);

            postsdb.Entry(post).State = EntityState.Modified;
            await postsdb.SaveChangesAsync();
            return Ok();
        }
    }
}