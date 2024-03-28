using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Identity;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Client.Services.Interfaces
{
    public interface IUserService : ITransient
    {
        Task<IResponseWrapper<List<UserResponse>>> GetAllAsync();

        Task<IResponseWrapper<UserResponse>> GetByIdAsync(string userId);

        Task<IResponseWrapper<List<UserRoleViewModel>>> GetRolesAsync(string userId);

        Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request);

        Task<IResponseWrapper<string>> UpdateUserAsync(UpdateUserRequest request);

        Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest request);

        Task<IResponseWrapper> UpdateRolesAsync(UpdateUserRolesRequest request);

        Task<IResponseWrapper> ChangePasswordAsync(ChangePasswordRequest model);
    }
}