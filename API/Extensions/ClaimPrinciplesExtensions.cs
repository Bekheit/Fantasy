using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimPrinciplesExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
