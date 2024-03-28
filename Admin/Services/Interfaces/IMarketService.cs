using SuperMarket.Common.Requests.Markets;
using SuperMarket.Common.Responses.Markets;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Client.Services.Interfaces
{
    public interface IMarketService : ITransient
    {
        Task<IResponseWrapper<MarketResponse>> CreateAsync(CreateMarketRequest request);

        Task<IResponseWrapper<MarketResponse>> UpdateAsync(UpdateMarketRequest request);

        Task<IResponseWrapper> DeleteAsync(int marketId);

        Task<IResponseWrapper<List<MarketResponse>>> GetAllAsync();

        Task<IResponseWrapper<MarketResponse>> GetByIdAsync(int employeeId);
    }
}