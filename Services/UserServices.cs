using ShoppingCart.Interface;
using ShoppingCart.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public class UserServices: IUserservice
    {
        private readonly string? _connectionString;

        public UserServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<IEnumerable<Users?>> GetUserListAsync()
        {

            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {

                return await dbConnection.QueryAsync<Users?>("USPGetUserDetails", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Users?> GetUserByIdAsync(int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);
                return await dbConnection.QuerySingleOrDefaultAsync<Users>(
                    "USPGetUserById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }


        public async Task<(bool Success, string Message)> CreteUser(Users user)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "I");
                    parameters.Add("@Name", user.Name);
                    parameters.Add("@EmailId", user.EmailId);
                    parameters.Add("@Address", user.Address);
                    parameters.Add("@Password", user.Password);
                    parameters.Add("@PhoneNo", user.PhoneNo);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 256);

                    await dbConnection.ExecuteAsync("USPInsertUpdateDeleteUser", parameters, commandType: CommandType.StoredProcedure);

                    bool success = parameters.Get<bool>("@Success");
                    string message = parameters.Get<string>("@Message");

                    return (success, message);

                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine(ex.Message);
                throw; // Optionally rethrow
            }
        }
            public async Task<(bool Success, string Message)> UpdateUserAsync(Users user)
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "U");
                    parameters.Add("@UserId", user.UserId);
                    parameters.Add("@Name", user.Name);
                    parameters.Add("@EmailId",user.EmailId);
                    parameters.Add("@Address",user.Address);
                    parameters.Add("@Password",user);
                    parameters.Add("@PhoneNo", user.PhoneNo);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 256);

                    await dbConnection.ExecuteAsync("USPInsertUpdateDeleteUser", parameters, commandType: CommandType.StoredProcedure);
                    bool success = parameters.Get<bool>("@Success");
                    string message = parameters.Get<string>("@Message");

                    return (success, message);

                }
            }
        public async Task<(bool Success, string Message)> DeleteUsersync(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Action", "D");
                parameters.Add("@UserId", id);
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 256);

                await dbConnection.ExecuteAsync("USPInsertUpdateDeleteUser", parameters, commandType: CommandType.StoredProcedure);
                bool success = parameters.Get<bool>("@Success");
                string message = parameters.Get<string>("@Message");

                return (success, message);
            }
        }
    }
    
}
