using ShoppingCart.Models;

namespace ShoppingCart.Interface
{
    public interface Iorderservices
    {
    
        Task<IEnumerable<OrderItems?>> GetAllOrderDetailsAsync();
        Task<IEnumerable<OrderItems?>> GetUserOrderDetailsAsync(int Id);
        Task PlaceOrderAsync(Orders orders);
    }
}
