using ShoppingCart.Interface;
using ShoppingCart.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using NuGet.Protocol.Plugins;
using Azure.Core;

namespace ShoppingCart.Services
{
    public class CartService:ICartService
    {
        private readonly string? _connectionString;

        public CartService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<CartItem?>> GetCartsByEmailIdAsync(string EmaiId)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {

                var parameters = new DynamicParameters();
                parameters.Add("@EmailId", EmaiId);
                return await dbConnection.QueryAsync<CartItem?>("USPGetCartItem", parameters,commandType: CommandType.StoredProcedure
                );
              
            }
        }
        public async Task<CartItem?> GetCartByIdAsync(PurchaseRequest request)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var sql = "SELECT * FROM Cart WHERE CartId = @CartId";
                return await dbConnection.QuerySingleOrDefaultAsync<CartItem>(sql, new { CartId = request.CartId });
            }
        }

        public async Task AddToCartAsync(Cart cart)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", cart.UserId);
                    parameters.Add("@productId", cart.ProductId);
                   
                    await dbConnection.ExecuteAsync("USPInsertCart", parameters, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine(ex.Message);
                throw; // Optionally rethrow
            }
        }

        public async Task RemoveFromCartAsync(int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var sql = "DELETE FROM Cart WHERE CartId = @CartId";
                await dbConnection.ExecuteAsync(sql, new { CartId = Id });
            }

            //using (IDbConnection dbConnectionS = new SqlConnection(_connectionString))
            //{
            //    dbConnectionS.Open();
            //    var sqlS = "DELETE FROM Cart WHERE CartId = @CartId";
            //    await dbConnectionS.ExecuteAsync(sqlS, new { CartId = Id });
            //}
        }
        public async Task<Product?> GetProductByCartIdAsync(int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var sql = "SELECT * FROM Cart WHERE CartId = @CartId";
                return await dbConnection.QuerySingleOrDefaultAsync<Product?>(sql, new { CartId = Id });
            }
        }

    }
}
