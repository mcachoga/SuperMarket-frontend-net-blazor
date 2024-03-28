using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using SuperMarket.Client.Extensions;
using SuperMarket.Common.Authorization;

namespace SuperMarket.Client.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; } = default!;
        [Inject]
        protected IAuthorizationService AuthService { get; set; } = default!;
        private bool _canViewUsers;
        private bool _canViewRoles;
        private bool _canViewMarkets;
        private bool _canViewProducts;
        private bool _canViewOrders;

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthState).User;
            _canViewUsers = await AuthService.HasPermissionAsync(user, AppFeature.Users, AppAction.Read);
            _canViewRoles = await AuthService.HasPermissionAsync(user, AppFeature.UserRoles, AppAction.Read);
            _canViewMarkets = await AuthService.HasPermissionAsync(user, AppFeature.Markets, AppAction.Read);
            _canViewProducts = await AuthService.HasPermissionAsync(user, AppFeature.Products, AppAction.Read);
            _canViewOrders = await AuthService.HasPermissionAsync(user, AppFeature.Orders, AppAction.Read);
        }
    }
}