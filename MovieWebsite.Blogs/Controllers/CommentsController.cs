using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Contains endpoints for Comments API
    /// </summary>
    [Route("comments")]
    public class CommentsController : Controller
    {
        /// <summary>
        /// An endpoint to get comments of post (3 levels)
        /// </summary>
        /// <param name="guid">Guid of post to get comments of</param>
        /// <returns>Array (tree) of comments <see cref="ShortComment"/></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Post not found</response>
        [ProducesResponseType(typeof(IEnumerable<ShortComment>),200)]
        [ProducesResponseType(404)]
        [HttpGet("post/{guid:Guid}")]
        public async Task<IActionResult> FetchPostComments(Guid guid)
        {
            await using var db = new BlogsContext();
            var post = await db.Posts
                .Include(x => x.Comments)
                .ThenInclude(x=>x.Comments)
                .ThenInclude(x=>x.Comments)
                .ThenInclude(x=>x.Comments)
                .FirstOrDefaultAsync(x => x.Guid == guid); //idk how to include it till the end so i'll just implement a function to get children
            if (post is null) return NotFound("Post not found");
            var comments = post.Comments.Select(x => new ShortComment(x));

            return Json(comments);
        }
        
        /// <summary>
        /// An endpoint to get comments of comments (3 levels)
        /// </summary>
        /// <param name="guid">Guid of comment to get comments of</param>
        /// <returns>Array (tree) of comments <see cref="ShortComment"/></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Comment not found</response>
        [ProducesResponseType(typeof(IEnumerable<ShortComment>),200)]
        [ProducesResponseType(404)]
        [HttpGet("comments/{guid:Guid}")]
        public async Task<IActionResult> FetchCommentComments(Guid guid)
        {
            await using var db = new BlogsContext();
            var comment = await db.Comments
                .Include(x => x.Comments)
                .ThenInclude(x=>x.Comments)
                .ThenInclude(x=>x.Comments)
                .ThenInclude(x=>x.Comments)
                .FirstOrDefaultAsync(x => x.Guid == guid); //idk how to include it till the end so i'll just implement a function to get children
            if (comment is null) return NotFound("Comment not found");
            var comments = comment.Comments.Select(x => new ShortComment(x));
            
            return Json(comments);
        }
        
        /// <summary>
        /// An endpoint to add comment to comment
        /// </summary>
        /// <param name="guid">Guid of parent comment</param>
        /// <param name="commentReq">Comment data</param>
        /// <returns>Nothing</returns>
        /// <response code="200">Comment successfully added</response>
        /// <response code="404">Comment not found</response>
        /// <response code="401">Authorize to use this endpoint</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Authorize]
        [HttpPut("comments/{guid:Guid}")]
        public async Task<IActionResult> AddCommentForComment(Guid guid, [FromBody] CommentPostBody commentReq)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();

            await using var db = new BlogsContext();
            var parentComment = db.Comments.FirstOrDefault(x => x.Guid == guid);
            if (parentComment is null) return NotFound();
            
            var comment = new Comment()
            {
                AuthorId = userId.Value,
                AttachmentUrls = commentReq.AttachmentUrls,
                Text = commentReq.Text
            };
            parentComment.Comments.Add(comment);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        /// <summary>
        /// An endpoint to add comment to post
        /// </summary>
        /// <param name="guid">Guid of parent post</param>
        /// <param name="commentReq">Comment data</param>
        /// <returns>Nothing</returns>
        /// <response code="200">Comment successfully added</response>
        /// <response code="404">Post not found</response>
        /// <response code="401">Authorize to use this endpoint</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [Authorize]
        [HttpPut("posts/{guid:Guid}")]
        public async Task<IActionResult> AddCommentForPost(Guid guid, [FromBody] CommentPostBody commentReq)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();

            await using var db = new BlogsContext();
            var post = db.Posts.FirstOrDefault(x => x.Guid == guid);
            if (post is null) return NotFound();
            
            var comment = new Comment()
            {
                AuthorId = userId.Value,
                AttachmentUrls = commentReq.AttachmentUrls,
                Text = commentReq.Text
            };
            post.Comments.Add(comment);
            await db.SaveChangesAsync();
            return Ok();
        }
        
        /// <summary>
        /// Body of comment endpoints
        /// </summary>
        public class CommentPostBody
        {
            /// <summary>
            /// Text of comment
            /// </summary>
            [Required]
            public string Text { get; set; }
            /// <summary>
            /// Attachments of comment
            /// </summary>
            [Required]
            public HashSet<string> AttachmentUrls { get; set; }
        }
    }
}