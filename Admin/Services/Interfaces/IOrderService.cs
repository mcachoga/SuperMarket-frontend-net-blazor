using SuperMarket.Common.Requests.Orders;
using SuperMarket.Common.Responses.Orders;
using SuperMarket.Common.Responses.Wrappers;

namespace SuperMarket.Client.Services.Interfaces
{
    public interface IOrderService : ITransient
    {
        Task<IResponseWrapper<OrderResponse>> CreateAsync(CreateOrderRequest request);

        Task<IResponseWrapper<OrderResponse>> UpdateAsync(UpdateOrderRequest request);

        Task<IResponseWrapper> DeleteAsync(int orderId);

        Task<IResponseWrapper<List<OrderResponse>>> GetAllAsync();

        Task<IResponseWrapper<OrderResponse>> GetByIdAsync(int orderId);
    }
}