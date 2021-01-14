using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            // first to get the hold of the user
            // this gives us the user's username form the token that API uses to authenticate the user
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
