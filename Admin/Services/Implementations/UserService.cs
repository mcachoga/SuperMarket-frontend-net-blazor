using SuperMarket.Client.Extensions;
using SuperMarket.Client.Services.Endpoints;
using SuperMarket.Client.Services.Interfaces;
using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Identity;
using SuperMarket.Common.Responses.Wrappers;
using System.Net.Http.Json;

namespace SuperMarket.Client.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiEndpoints _apiEndpoints;

        public UserService(HttpClient httpClient, ApiEndpoints apiEndpoints)
        {
            _httpClient = httpClient;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<IResponseWrapper> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(_apiEndpoints.UserEndpoints.ChangePassword, request);
            return await response.WrapResponse();
        }

        public async Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(_apiEndpoints.UserEndpoints.ChangeStatus, request);
            return await response.WrapResponse();
        }

        public async Task<IResponseWrapper<List<UserResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(_apiEndpoints.UserEndpoints.GetAll);
            return await response.WrapResponse<List<UserResponse>>();
        }

        public async Task<IResponseWrapper<UserResponse>> GetByIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{_apiEndpoints.UserEndpoints.GetById}{userId}");
            return await response.WrapResponse<UserResponse>();
        }

        public async Task<IResponseWrapper<List<UserRoleViewModel>>> GetRolesAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{_apiEndpoints.UserEndpoints.GetRoles}{userId}");
            return await response.WrapResponse<List<UserRoleViewModel>>();
        }

        public async Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiEndpoints.UserEndpoints.Register, request);
            return await response.WrapResponse();
        }

        public async Task<IResponseWrapper> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(_apiEndpoints.UserEndpoints.UpdateRoles, request);
            return await response.WrapResponse();
        }

        public async Task<IResponseWrapper<string>> UpdateUserAsync(UpdateUserRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(_apiEndpoints.UserEndpoints.Update, request);
            return await response.WrapResponse<string>();
        }
    }
}
