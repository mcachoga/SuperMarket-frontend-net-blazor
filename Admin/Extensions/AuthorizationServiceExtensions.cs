using Microsoft.AspNetCore.Authorization;
using SuperMarket.Common.Authorization;
using System.Security.Claims;

namespace SuperMarket.Client.Extensions
{
    public static class AuthorizationServiceExtensions
    {
        public static async Task<bool> HasPermissionAsync(this IAuthorizationService service, ClaimsPrincipal user, string feature, string action) 
            => (await service.AuthorizeAsync(user, null, AppPermission.NameFor(feature, action))).Succeeded;
    }
}