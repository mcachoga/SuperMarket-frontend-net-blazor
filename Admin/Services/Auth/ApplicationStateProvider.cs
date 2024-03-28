﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SuperMarket.Common.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace SuperMarket.Client.Services.Auth
{
    public class ApplicationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public ClaimsPrincipal AuthenticationStateUser { get; set; }

        public ApplicationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public void MarkUserAsAuthenticated(string userName)
        {
            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }, "apiauth"));

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
        {
            var state = await GetAuthenticationStateAsync();
            var authenticationStateProviderUser = state.User;
            return authenticationStateProviderUser;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>(StorageConstants.AuthToken);
            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
            var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));
            AuthenticationStateUser = state.User;
            return state;
        }

        private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

                if (roles != null)
                {
                    if (roles.ToString().Trim().StartsWith("["))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                        claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                    }

                    keyValuePairs.Remove(ClaimTypes.Role);
                }

                keyValuePairs.TryGetValue(AppClaim.Permission, out var permissions);
                if (permissions != null)
                {
                    if (permissions.ToString().Trim().StartsWith("["))
                    {
                        var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString());
                        claims.AddRange(parsedPermissions.Select(permission => new Claim(AppClaim.Permission, permission)));
                    }
                    else
                    {
                        claims.Add(new Claim(AppClaim.Permission, permissions.ToString()));
                    }
                    keyValuePairs.Remove(AppClaim.Permission);
                }

                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            }
            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
