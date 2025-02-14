using ShoppingCart.Models;

namespace ShoppingCart.Interface
{
    public interface IUserservice
    {

        Task<(bool Success, string Message)> CreteUser(Users user);
        Task<(bool Success, string Message)> UpdateUserAsync(Users user);
        Task<Users?> GetUserByIdAsync(int Id);
        Task<IEnumerable<Users?>> GetUserListAsync();
        Task<(bool Success, string Message)> DeleteUsersync(int id);

       
    }
}
