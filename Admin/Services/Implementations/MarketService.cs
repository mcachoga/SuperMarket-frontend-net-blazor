using SuperMarket.Client.Extensions;
using SuperMarket.Client.Services.Endpoints;
using SuperMarket.Client.Services.Interfaces;
using SuperMarket.Common.Requests.Markets;
using SuperMarket.Common.Responses.Markets;
using SuperMarket.Common.Responses.Wrappers;
using System.Net.Http.Json;

namespace SuperMarket.Client.Services.Implementations
{
    public class MarketService : IMarketService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiEndpoints _apiEndpoints;

        public MarketService(HttpClient httpClient, ApiEndpoints apiEndpoints)
        {
            _httpClient = httpClient;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<IResponseWrapper<MarketResponse>> CreateAsync(CreateMarketRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiEndpoints.MarketEndpoints.Create, request);
            return await response.WrapResponse<MarketResponse>();
        }

        public async Task<IResponseWrapper> DeleteAsync(int employeeId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiEndpoints.MarketEndpoints.Delete}{employeeId}");
            return await response.WrapResponse<MarketResponse>();
        }

        public async Task<IResponseWrapper<List<MarketResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(_apiEndpoints.MarketEndpoints.GetAll);
            return await response.WrapResponse<List<MarketResponse>>();
        }

        public async Task<IResponseWrapper<MarketResponse>> GetByIdAsync(int marketId)
        {
            var response = await _httpClient.GetAsync($"{_apiEndpoints.MarketEndpoints.GetById}{marketId}");
            return await response.WrapResponse<MarketResponse>();
        }

        public async Task<IResponseWrapper<MarketResponse>> UpdateAsync(UpdateMarketRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(_apiEndpoints.MarketEndpoints.Update, request);
            return await response.WrapResponse<MarketResponse>();
        }
    }
}