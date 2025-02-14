using Azure.Core;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ShoppingCart.Models;
namespace ShoppingCart.Interface
{
    public interface ICartService
    {
        //Task<IEnumerable<CartItem?>> GetCartsByUserIdAsync(int userId);
        Task<CartItem?> GetCartByIdAsync(PurchaseRequest request);
        //Task GetCartBycartIdAsync(Int Id);
        Task<IEnumerable<CartItem?>> GetCartsByEmailIdAsync(string EmaiId);
        Task RemoveFromCartAsync(int Id);
        Task<Product?> GetProductByCartIdAsync(int Id);
        Task AddToCartAsync(Cart cart);
    }
}
