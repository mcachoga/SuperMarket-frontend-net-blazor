using SuperMarket.Client.Extensions;
using SuperMarket.Client.Services.Auth;
using SuperMarket.Client.Services.Endpoints;
using SuperMarket.Client.Services.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Identity;
using SuperMarket.Common.Responses.Wrappers;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace SuperMarket.Client.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ApiEndpoints _apiEndpoints;

        public TokenService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider, 
            ApiEndpoints apiEndpoints)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<IResponseWrapper> Login(TokenRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiEndpoints.AuthEndpoints.GetToken, request);
            var result = await response.WrapResponse<TokenResponse>();
            if (result.IsSuccessful)
            {
                var token = result.ResponseData.Token;
                var refreshToken = result.ResponseData.RefreshToken;

                await _localStorage.SetItemAsync(StorageConstants.AuthToken, token);
                await _localStorage.SetItemAsync(StorageConstants.RefreshToken, refreshToken);
                
                ((ApplicationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(request.Email);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await ResponseWrapper.SuccessAsync();
            }
            else
            {
                return await ResponseWrapper.FailAsync(result.Messages);
            }
        }

        public async Task<IResponseWrapper> Logout()
        {
            await _localStorage.RemoveItemAsync(StorageConstants.AuthToken);
            await _localStorage.RemoveItemAsync(StorageConstants.RefreshToken);

            ((ApplicationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return await ResponseWrapper.SuccessAsync();
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>(StorageConstants.AuthToken);
            var refreshToken = await _localStorage.GetItemAsync<string>(StorageConstants.RefreshToken);

            var response = await _httpClient.PostAsJsonAsync(_apiEndpoints.AuthEndpoints.GetRefreshToken, 
                new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });

            var result = await response.WrapResponse<TokenResponse>();

            if (!result.IsSuccessful)
            {
                throw new ApplicationException("Something went wrong during the refresh token action");
            }

            token = result.ResponseData.Token;
            refreshToken = result.ResponseData.RefreshToken;
            await _localStorage.SetItemAsync(StorageConstants.AuthToken, token);
            await _localStorage.SetItemAsync(StorageConstants.RefreshToken, refreshToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return token;
        }

        public async Task<string> TryForceRefreshToken()
        {
            //check if token exists
            var availableToken = await _localStorage.GetItemAsync<string>(StorageConstants.RefreshToken);
            if (string.IsNullOrEmpty(availableToken)) return string.Empty;
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;

            //On your last 5 minute before token expires, we refresh your token.
            if (diff.TotalMinutes <= 5)
                return await RefreshToken();
            return string.Empty;
        }

        public async Task<string> TryRefreshToken()
        {
            return await RefreshToken();
        }
    }
}
