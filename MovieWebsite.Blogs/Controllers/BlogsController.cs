using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Blogs.Models.Databases;
using MovieWebsite.Shared;

namespace MovieWebsite.Blogs.Controllers
{
    [Route("blogs")]
    public class BlogsController : Controller
    {
        [HttpGet("{guid:Guid}")]
        public async Task<IActionResult> FetchPostByGuid(Guid guid)
        {
            await using var db = new BlogsContext();
            var draft = await db.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == guid);
            return Json(draft);
        }

        [HttpGet("users/{userId:int}")]
        public async Task<IActionResult> FetchUserPosts(int userId)
        {
            await using var db = new BlogsContext();
            var draft = await db.Posts.AsNoTracking().Where(x => x.AuthorId == userId).ToArrayAsync();
            return Json(draft);
        }
        
        [HttpGet("popular")]
        public async Task<IActionResult> FetchPopularPosts(uint amount, uint skip)
        {
            throw new NotImplementedException();
        }
        
        [HttpGet("latest")]
        public async Task<IActionResult> FetchLatestPosts(uint amount, uint skip)
        {
            await using var db = new BlogsContext();
            return Json(await db.Posts.AsNoTracking().OrderBy(x=>x.TimeStamp).Skip((int)skip).Take((int)amount).ToArrayAsync());
        }
        
        [Authorize]
        [HttpPut("add")]
        public async Task<IActionResult> AddPost(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var draftsdb = new DraftsContext();
            var draft = await draftsdb.Drafts.AsNoTracking().FirstOrDefaultAsync(x => x.Guid == guid);
            if (draft.AuthorId != userId) return Forbid();

            await using var postsdb = new BlogsContext();
            draft.TimeStamp = DateTime.Now;
            await postsdb.AddAsync(draft);
            var postTask = postsdb.SaveChangesAsync();
            
            draftsdb.Remove(draft); 
            await draftsdb.SaveChangesAsync();
            
            await postTask;
            
            return Ok();
        }

        //TODO: that shouldn't clear post ID to prevent accidental links
        [Authorize]
        [HttpDelete("remove")]
        public async Task<IActionResult> DeletePost()
        {
            throw new NotImplementedException();
        }
        
        [Authorize]
        [HttpGet("{guid:Guid}/rankup")]
        public async Task<IActionResult> RankPostUp(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();

            var isModified = false;
            await using var postsdb = new BlogsContext();
            var post = await postsdb.Posts.FirstOrDefaultAsync(x => x.Guid == guid);
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
        [Authorize]
        [HttpGet("{guid:Guid}/rankdown")]
        public async Task<IActionResult> RankPostDown(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();

            var isModified = false;
            await using var postsdb = new BlogsContext();
            var post = await postsdb.Posts.FirstOrDefaultAsync(x => x.Guid == guid);
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
        
        [Authorize]
        [HttpGet("{guid:Guid}/rankclean")]
        public async Task<IActionResult> CleanRankPost(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            await using var postsdb = new BlogsContext();
            var post = await postsdb.Posts.FirstOrDefaultAsync(x => x.Guid == guid);

            if (post.RankDowners.Contains(userId.Value)) post.RankDowners.Remove(userId.Value);
            if (post.RankUppers.Contains(userId.Value)) post.RankUppers.Remove(userId.Value);

            postsdb.Entry(post).State = EntityState.Modified;
            await postsdb.SaveChangesAsync();
            return Ok();
        }
    }
}