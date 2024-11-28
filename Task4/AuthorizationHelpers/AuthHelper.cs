using System.Security.Claims;
using Task4.Db;

namespace Task4.AuthorizationHelpers
{
    public static class AuthHelper
    {
        public const string AUTH_COOKIE = "CookieAuth";
        public const string ANTI_FORGERY = "Requesttoken";

        public static ClaimsPrincipal GetClaimsPrincipal(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };
            if (!user.IsBlocked)
            {
                claims.Add(new Claim(AppClaims.ADMIN, ""));
            }
            var identity = new ClaimsIdentity(claims, AUTH_COOKIE);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            return claimsPrincipal;
        }

        public static string GetWebClientEmail(ClaimsPrincipal claimsPrincipal)
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            return identity?.FindFirst(ClaimTypes.Name)?.Value ?? "";
        }
    }
}
