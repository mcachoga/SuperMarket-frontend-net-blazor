using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SuperMarket.Client.Extensions;
using SuperMarket.Common.Authorization;
using SuperMarket.Common.Responses.Products;

namespace SuperMarket.Client.Pages.Catalog
{
    public partial class ProductList
    {
        [CascadingParameter]
        protected Task<AuthenticationState> AuthState { get; set; } = default!;
        [Inject]
        protected IAuthorizationService AuthService { get; set; } = default!;

        private List<ProductResponse> _productList = new();
        
        private ProductResponse _product = new();

        private bool _canCreateProducts;
        private bool _canUpdateProducts;
        private bool _canDeleteProducts;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthState).User;
            _canCreateProducts = await AuthService.HasPermissionAsync(user, AppFeature.Products, AppAction.Create);
            _canUpdateProducts = await AuthService.HasPermissionAsync(user, AppFeature.Products, AppAction.Update);
            _canDeleteProducts = await AuthService.HasPermissionAsync(user, AppFeature.Products, AppAction.Delete);

            await GetMarketsAsync();
            _loaded = true;
        }

        private async Task GetMarketsAsync()
        {
            var response = await _productService.GetAllAsync();
            if (response.IsSuccessful)
            {
                _productList = response.ResponseData;
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