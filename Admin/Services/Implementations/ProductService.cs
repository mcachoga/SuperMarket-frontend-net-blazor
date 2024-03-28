using SuperMarket.Client.Extensions;
using SuperMarket.Client.Services.Endpoints;
using SuperMarket.Client.Services.Interfaces;
using SuperMarket.Common.Requests.Products;
using SuperMarket.Common.Responses.Products;
using SuperMarket.Common.Responses.Wrappers;
using System.Net.Http.Json;

namespace SuperMarket.Client.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiEndpoints _apiEndpoints;

        public ProductService(HttpClient httpClient, ApiEndpoints apiEndpoints)
        {
            _httpClient = httpClient;
            _apiEndpoints = apiEndpoints;
        }

        public async Task<IResponseWrapper<ProductResponse>> CreateAsync(CreateProductRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiEndpoints.ProductEndpoints.Create, request);
            return await response.WrapResponse<ProductResponse>();
        }

        public async Task<IResponseWrapper> DeleteAsync(int employeeId)
        {
            var response = await _httpClient.DeleteAsync($"{_apiEndpoints.ProductEndpoints.Delete}{employeeId}");
            return await response.WrapResponse<ProductResponse>();
        }

        public async Task<IResponseWrapper<List<ProductResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(_apiEndpoints.ProductEndpoints.GetAll);
            return await response.WrapResponse<List<ProductResponse>>();
        }

        public async Task<IResponseWrapper<ProductResponse>> GetByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"{_apiEndpoints.ProductEndpoints.GetById}{productId}");
            return await response.WrapResponse<ProductResponse>();
        }

        public async Task<IResponseWrapper<ProductResponse>> UpdateAsync(UpdateProductRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync(_apiEndpoints.ProductEndpoints.Update, request);
            return await response.WrapResponse<ProductResponse>();
        }
    }
}