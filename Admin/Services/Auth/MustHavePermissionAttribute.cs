using Microsoft.AspNetCore.Authorization;
using SuperMarket.Common.Authorization;

namespace SuperMarket.Client.Services.Auth
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string action, string resource) => Policy = AppPermission.NameFor(action, resource);
    }
}