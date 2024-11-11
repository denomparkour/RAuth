using Microsoft.AspNetCore.Http;
using RAuth.Application.Constants;
using RAuth.Core.Exceptions;
using System.Security.Claims;

namespace RAuth.Application.Util
{
    public  class ExtractUserId
    {
        public static string Extract(IHttpContextAccessor httpContext)
        {
            if (httpContext.HttpContext == null)
            {
                throw new FailedException(GlobalConstants.UNAUTHORIZED);
            }
            return httpContext.HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
        }
    }
}
