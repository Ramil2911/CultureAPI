using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MovieWebsite.Shared
{
    public static class Helpers
    {
        public static int? GetIdClaim(this ClaimsIdentity identity)
        {
            int userId;
            try
            {
                userId = int.Parse(identity.Claims.FirstOrDefault(x => x.Type == "id").Value);
            }
            catch
            {
                return null;
            }

            return userId;
        }
        public static int? GetClaim(this ClaimsIdentity identity, string claimType)
        {
            int userId;
            try
            {
                userId = int.Parse(identity.Claims.FirstOrDefault(x => x.Type == claimType).Value);
            }
            catch
            {
                return null;
            }

            return userId;
        }
    }
}