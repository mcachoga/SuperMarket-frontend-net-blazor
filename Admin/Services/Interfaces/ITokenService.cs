using SuperMarket.Common.Requests.Identity;
using SuperMarket.Common.Responses.Wrappers;
using System.Security.Claims;

namespace SuperMarket.Client.Services.Interfaces
{
    public interface ITokenService : ITransient
    {
        Task<IResponseWrapper> Login(TokenRequest request);

        Task<IResponseWrapper> Logout();

        Task<string> RefreshToken();

        Task<string> TryRefreshToken();

        Task<string> TryForceRefreshToken();

        Task<ClaimsPrincipal> CurrentUser();
    }
}