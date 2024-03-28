using SuperMarket.Common.Requests.Products;
using SuperMarket.Common.Responses.Products;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Client.Services.Interfaces
{
    public interface IProductService : ITransient
    {
        Task<IResponseWrapper<ProductResponse>> CreateAsync(CreateProductRequest request);

        Task<IResponseWrapper<ProductResponse>> UpdateAsync(UpdateProductRequest request);

        Task<IResponseWrapper> DeleteAsync(int orderId);

        Task<IResponseWrapper<List<ProductResponse>>> GetAllAsync();

        Task<IResponseWrapper<ProductResponse>> GetByIdAsync(int orderId);
    }
}