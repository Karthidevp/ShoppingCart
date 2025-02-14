using ShoppingCart.Models;
namespace ShoppingCart.Interface
{
    public interface IProductServices
    {
        Task InsertOrUpdateProductAsync(Product product);
        //Task<int> CreateProduct(Product product);
        Task<Product?> GetProductByIdAsync(int productId);
    
        Task<IEnumerable<Product>> GetAllProductsAsync();
        //Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProductAsync(int id);

        Task ActivateProductAsync(int id);
        Task InActivateProductAsync(int id);
       Task AddToCartProductAsync(int id);

    }
}
