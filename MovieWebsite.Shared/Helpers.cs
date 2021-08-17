using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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
    }
}