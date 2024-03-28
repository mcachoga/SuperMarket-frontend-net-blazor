using SuperMarket.Client.Extensions;
using SuperMarket.Client.Services.Endpoints;
using SuperMarket.Client.Services.Interfaces;
using SuperMarket.Common.Requests.Orders;
using SuperMarket.Common.Responses.Orders;
using SuperMarket.Common.Responses.Wrappers;
using System.Net.Http.Json;

namespace SuperMarket.Client.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiEndpoints _apiEndpoints;

        public OrderService(HttpClient httpClient, ApiEndpoints apiEndpoints)
        {
            _httpClient = httpClient;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<IResponseWrapper<OrderResponse>> CreateAsync(CreateOrderRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiEndpoints.OrderEndpoints.Create, request);
            return await response.WrapResponse<OrderResponse>();
        }

        public async Task<IResponseWrapper> DeleteAsync(int employeeId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiEndpoints.OrderEndpoints.Delete}{employeeId}");
            return await response.WrapResponse<OrderResponse>();
        }

        public async Task<IResponseWrapper<List<OrderResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(_apiEndpoints.OrderEndpoints.GetAll);
            return await response.WrapResponse<List<OrderResponse>>();
        }

        public async Task<IResponseWrapper<OrderResponse>> GetByIdAsync(int orderId)
        {
            var response = await _httpClient.GetAsync($"{_apiEndpoints.OrderEndpoints.GetById}{orderId}");
            return await response.WrapResponse<OrderResponse>();
        }

        public async Task<IResponseWrapper<OrderResponse>> UpdateAsync(UpdateOrderRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(_apiEndpoints.OrderEndpoints.Update, request);
            return await response.WrapResponse<OrderResponse>();
        }
    }
}