using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            // ℹ️ Gets the username for the current logged user, based on the token in jwt service
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}