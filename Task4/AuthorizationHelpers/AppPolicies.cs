using Microsoft.AspNetCore.Authorization;
using Syncfusion.EJ2.Charts;

namespace Task4.AuthorizationHelpers
{
    public static class AppPolicies
    {
        public const string ADMIN_POLICY = "AdminOnly";

        public static void SetAdminPolicy(AuthorizationOptions options)
        {
            options.AddPolicy(ADMIN_POLICY, policy =>
            {
                policy.RequireClaim(AppClaims.ADMIN);
            });
        }
    }
}
