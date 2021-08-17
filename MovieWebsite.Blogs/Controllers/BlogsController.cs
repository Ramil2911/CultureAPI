using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieWebsite.Blogs.Controllers
{
    [Route("blogs")]
    public class BlogsController : Controller
    {
        [HttpGet("{guid:Guid}")]
        public async Task<IActionResult> FetchPostByGuid(Guid guid)
        {
            throw new NotImplementedException();
        }

        [HttpGet("users/{userId:int}")]
        public async Task<IActionResult> FetchUserPosts(int userId)
        {
            throw new NotImplementedException();
        }
        
        [HttpGet("popular")]
        public async Task<IActionResult> FetchPopularPosts(uint amount)
        {
            throw new NotImplementedException();
        }
        
        [Authorize]
        [HttpPut("add")]
        public async Task<IActionResult> AddPost()
        {
            throw new NotImplementedException();
        }

        //TODO: that shouldn't clear post ID to prevent accidental links
        [Authorize]
        [HttpDelete("remove")]
        public async Task<IActionResult> DeletePost()
        {
            throw new NotImplementedException();
        }
    }
}