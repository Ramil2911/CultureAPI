using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MovieWebsite.Shared
{
    /// <summary>
    /// Attribute which allows access to endpoint only for ips from safelist
    /// </summary>
    public class InternalAttribute : ActionFilterAttribute
    {
        private readonly string[] _safelist = new[]
        {
            "127.0.0.1"
        };

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var remoteIp = context.HttpContext.Connection.RemoteIpAddress;

            if (remoteIp.IsIPv4MappedToIPv6)
            {
                remoteIp = remoteIp.MapToIPv4();
            }

            var badIp = !_safelist.Select(IPAddress.Parse).Contains(remoteIp);

            if (badIp)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}