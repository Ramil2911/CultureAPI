using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieWebsite.Accounts.Models.Databases;
using MovieWebsite.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieWebsite.Accounts.Models;

namespace MovieWebsite.Accounts.Controllers
{
    /// <summary>
    /// Contains endpoints for Identity API
    /// </summary>
    public class AuthController : Controller
    {
        /// <summary>
        /// A login endpoint
        /// </summary>
        /// <param name="username">User's username</param>
        /// <param name="password">User's password</param>
        /// <returns>An json object with access_token, refresh_token and username <see cref="AuthResponse"/></returns>
        /// <response code="400">Invalid username or password</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type=typeof(AuthResponse))]
        [HttpGet("/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            await using var db = new AccountContext();
            
            var account = await db.Accounts.FirstOrDefaultAsync(x => x.Username == username);
            if (!VerifyPassword(password, account)) return BadRequest(new { errorText = "Invalid username or password." });
            
            var identity = GetIdentity(account);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
 
            var (accessJwt, refreshJwt) = GenerateTokens(identity);

            var response = new AuthResponse()
            {
                access_token = accessJwt,
                refresh_token = refreshJwt,
                username = identity.Name
            };
 
            return Json(response);
        }

        /// <summary>
        /// An endpoint to refresh by refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>An json object with new access_token, refresh_token and username <see cref="AuthResponse"/></returns>
        /// <response code="400">Invalid refresh token</response>
        /// <response code="200">Success</response>
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type=typeof(AuthResponse))]
        [HttpGet("/refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal claims;
            try
            {
                claims = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    // укзывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = RefreshOptions.ISSUER,

                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = RefreshOptions.AUDIENCE,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,

                    // установка ключа безопасности
                    IssuerSigningKey = RefreshOptions.GetSymmetricSecurityKey(),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                }, out _);
            }
            catch (Exception e)
            {
                return BadRequest("Refresh token is invalid");
            }

            var idSuccess = int.TryParse(claims.FindFirstValue("id"), out var userId);
            if (!idSuccess) return BadRequest("Bad jwt's payload");

            await using var db = new AccountContext();

            var account = await db.Accounts.FirstOrDefaultAsync(x => x.Id == userId);
            if (account is null) return BadRequest("Bad jwt's payload");

            var identity = GetIdentity(account);
            var (accessJwt, refreshJwt) = GenerateTokens(identity);

            var response = new AuthResponse()
            {
                access_token = accessJwt,
                refresh_token = refreshJwt,
                username = identity.Name
            };
 
            return Json(response);
        }

        [HttpPut("signup")]
        public async Task<IActionResult> Register(string username, string password, string email)
        {
            throw new NotImplementedException();
            //TODO: signing up
        }
 
        private ClaimsIdentity GetIdentity(Account account)
        {
            if (account == null) return null;
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, account.Username),
                new(ClaimsIdentity.DefaultRoleClaimType, account.Role),
                new("role", account.Role),
                new("id", account.Id.ToString())
            };
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            var hash = pbkdf2.GetBytes(20);
            
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            
            return Convert.ToBase64String(hashBytes);
        }

        private bool VerifyPassword(string password, Account account)
        {
            /* Fetch the stored value */
            var savedPasswordHash = account.PasswordHash;
            
            /* Extract the bytes */
            var hashBytes = Convert.FromBase64String(savedPasswordHash);
            
            /* Get the salt */
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            var hash = pbkdf2.GetBytes(20);
            
            /* Compare the results */
            for (var i = 0; i < 20; i++)
                if (hashBytes[i+16] != hash[i])
                    return false;
            return true;
        }

        private (string, string) GenerateTokens(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            
            var jwt1 = new JwtSecurityToken( //access
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var jwt2 = new JwtSecurityToken( //refresh
                issuer: RefreshOptions.ISSUER,
                audience: RefreshOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(RefreshOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(RefreshOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return (new JwtSecurityTokenHandler().WriteToken(jwt1), new JwtSecurityTokenHandler().WriteToken(jwt2));
        }

        /// <summary>
        /// Standard response for successful authorization
        /// </summary>
        private class AuthResponse
        {
            // ReSharper disable thrice InconsistentNaming
            /// <summary>
            /// Jwt access token;
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// Jwt refresh token
            /// </summary>
            public string refresh_token { get; set; }
            /// <summary>
            /// User's username
            /// </summary>
            public string username { get; set; }
        }
    }
}