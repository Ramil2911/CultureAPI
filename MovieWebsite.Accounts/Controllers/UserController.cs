using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Accounts.Models.Databases;
using MovieWebsite.Shared;

namespace MovieWebsite.Accounts.Controllers
{
    /// <summary>
    /// Contains endpoints for Accounts API
    /// </summary>
    [Route("user")]
    public class UserController : Controller
    {
        private IHttpClientFactory _clientFactory;
        
        public UserController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> FetchUser(int userId)
        {
            await using var db = new AccountContext();
            var user = await db.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
            return Json(user.Short);
        }
        
        /// <summary>
        /// [INTERNAL] Increase user's rank by 1
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>Nothing</returns>
        /// <response code="200">Success</response>
        /// <response code="403">You're not allowed to run internal functions</response>
        [ProducesResponseType(403)]
        [ProducesResponseType(200)]
        [Internal]
        [HttpHead("rankup")]
        public async Task<IActionResult> RankUp(int userId)
        {
            await using var db = new AccountContext();
            var user = await db.Accounts.FirstOrDefaultAsync(x => x.Id == userId);
            user.Karma++;
            await db.SaveChangesAsync();
            return Ok();
        }
        
        /// <summary>
        /// [INTERNAL] Decrease user's rank by 1
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>Nothing</returns>
        /// <response code="200">Success</response>
        /// <response code="403">You're not allowed to run internal functions</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [Internal]
        [HttpHead("rankdown")]
        public async Task<IActionResult> RankDown(int userId)
        {
            await using var db = new AccountContext();
            var user = await db.Accounts.FirstOrDefaultAsync(x => x.Id == userId);
            user.Karma--;
            await db.SaveChangesAsync();
            return Ok();
        }
        
        
        /// <summary>
        /// Sets avatar of user
        /// </summary>
        /// <param name="guid">Guid of image on image server</param>
        /// <returns>Success status code if avatar changed</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Image not found on server</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Authorize]
        [HttpPut("avatar")]
        public async Task<IActionResult> SetAvatar(Guid guid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.GetIdClaim();
            if (!userId.HasValue) return Unauthorized();
            
            var httpClient = _clientFactory.CreateClient();
            var result = await httpClient.GetAsync(ServerIps.Value[4] + "image/check" + "?guid=" + guid);
            if (!result.IsSuccessStatusCode) return BadRequest();
            
            await using var db = new AccountContext();
            var account = await db.Accounts.FirstOrDefaultAsync(x => x.Id == userId);
            account.AvatarGuid = guid;
            return Ok();

        }
    }
}