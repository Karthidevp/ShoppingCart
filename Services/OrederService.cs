using ShoppingCart.Interface;
using ShoppingCart.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace ShoppingCart.Services
{
    public class OrederService : Iorderservices
    {
        private readonly string? _connectionString;

        public OrederService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<OrderItems?>> GetAllOrderDetailsAsync()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {

                return await dbConnection.QueryAsync<OrderItems?>("USPOrderDetails", commandType: CommandType.StoredProcedure
                );

            }
        }
        public async Task<IEnumerable<OrderItems?>> GetUserOrderDetailsAsync(int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", Id);
                return await dbConnection.QueryAsync<OrderItems?>("USPUserOrderDetails", parameters, commandType: CommandType.StoredProcedure
                );

            }
        }
        //public async Task<bool> PurchaseAsync(CartItem cartItem)
        //{
        //    try
        //    {
        //        using (IDbConnection dbConnection = new SqlConnection(_connectionString))
        //        {
        //            dbConnection.Open();
        //            var parameters = new DynamicParameters();
        //            parameters.Add("@ProductId", cartItem.ProductId);
        //            parameters.Add("@Userid", cartItem.UserId);
        //            parameters.Add("@quantity", cartItem.QNTY);
        //            parameters.Add("@TotalAmount", cartItem.TotalAmount);
        //            parameters.Add("@cartId", cartItem.CartId);

        //            // Execute the stored procedure and get the number of affected rows
        //            var rowsAffected = await dbConnection.ExecuteAsync("USPInsertOrder", parameters, commandType: CommandType.StoredProcedure);

        //            // Return true if at least one row was affected
        //            return rowsAffected > 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log or handle the exception as needed
        //        Console.WriteLine(ex.Message);
        //        // Optionally rethrow or return false if preferred
        //        throw;
        //    }
        //}

        public async Task PlaceOrderAsync(Orders orders)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@CartId", orders.cartid);
                    parameters.Add("@UserId", orders.Userid);
                    parameters.Add("@ProductId", orders.productid);
                    parameters.Add("@Quantity", orders.Quantity);
                    parameters.Add("@TotalAmount", orders.Totalamout);
                     await dbConnection.ExecuteAsync("USPPlaceOrder", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine(ex.Message);
                // Optionally rethrow or return false if preferred
                throw;

            }

        }
    }
}
