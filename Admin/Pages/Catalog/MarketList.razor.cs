using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SuperMarket.Client.Extensions;
using SuperMarket.Common.Authorization;
using SuperMarket.Common.Responses.Markets;

namespace SuperMarket.Client.Pages.Catalog
{
    public partial class MarketList
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; } = default!;
        [Inject]
        protected IAuthorizationService AuthService { get; set; } = default!;

        private List<MarketResponse> _marketList = new();
        private MarketResponse _market = new();

        private bool _canCreateMarkets;
        private bool _canUpdateMarkets;
        private bool _canDeleteMarkets;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthState).User;
            _canCreateMarkets = await AuthService.HasPermissionAsync(user, AppFeature.Markets, AppAction.Create);
            _canUpdateMarkets = await AuthService.HasPermissionAsync(user, AppFeature.Markets, AppAction.Update);
            _canDeleteMarkets = await AuthService.HasPermissionAsync(user, AppFeature.Markets, AppAction.Delete);

            await GetMarketsAsync();
            _loaded = true;
        }

        private async Task GetMarketsAsync()
        {
            var response = await _marketService.GetAllAsync();
            if (response.IsSuccessful)
            {
                _marketList = response.ResponseData;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void Cancel()
        {
            _navigationManager.NavigateTo("/");
        }
    }
}