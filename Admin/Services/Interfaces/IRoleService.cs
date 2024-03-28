using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Identity;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Client.Services.Interfaces
{
    public interface IRoleService : ITransient
    {
        Task<IResponseWrapper<List<RoleResponse>>> GetRolesAsync();

        Task<IResponseWrapper<string>> CreateAsync(CreateRoleRequest request);

        Task<IResponseWrapper<string>> UpdateAsync(UpdateRoleRequest request);

        Task<IResponseWrapper<string>> DeleteAsync(string roleId);

        Task<IResponseWrapper<RoleClaimResponse>> GetPermissionsAsync(string roleId);

        Task<IResponseWrapper<string>> UpdatePermissionsAsync(UpdateRolePermissionsRequest request);
    }
}