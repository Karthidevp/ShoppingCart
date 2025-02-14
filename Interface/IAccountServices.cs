using ShoppingCart.Models;

namespace ShoppingCart.Interface
{
    public interface IAccountServices
    {

        Task<(bool Success, string Message, string Role, int Id)> LoginUserAsync(Login login);
        Task<(bool Success, string Message, string Role)> AdminLoginAsync(Login login);
        Task<Users> GetUserByIdAync(string emailid);
    }
}
