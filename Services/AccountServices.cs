using ShoppingCart.Interface;
using ShoppingCart.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
namespace ShoppingCart.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly string? _connectionString;

        public AccountServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task<Users> GetUserByIdAync(string emailid)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@EmailId", emailid);
                return await dbConnection.QueryFirstOrDefaultAsync<Users>("GetUserByemailId", parameters, commandType: CommandType.StoredProcedure);
            }

        }
        #region  User Login
        public async Task<(bool Success, string Message, string Role,int Id)> LoginUserAsync(Login login)
        
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@EmailId", login.EmailId);
            parameters.Add("@Password", login.Password); // Ideally hashed and salted
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 256);
            parameters.Add("@Role", dbType: DbType.String, direction: ParameterDirection.Output, size: 256);
            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await connection.ExecuteAsync("LoginUser", parameters, commandType: CommandType.StoredProcedure);

            bool success = parameters.Get<bool>("@Success");
            string message = parameters.Get<string>("@Message");
            string role = parameters.Get<string>("@Role");
            int Userid = parameters.Get<int>("@Id");


            return (success, message, role, Userid);
        }
        #endregion
        #region  Admin Login 
        public async Task<(bool Success, string Message, string Role)> AdminLoginAsync(Login login)

        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@EmailId", login.EmailId);
            parameters.Add("@Password", login.Password); // Ideally hashed and salted
            parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 256);
            parameters.Add("@Role", dbType: DbType.String, direction: ParameterDirection.Output, size: 256);
          
            await connection.ExecuteAsync("USPAdminLogin", parameters, commandType: CommandType.StoredProcedure);

            bool Success = parameters.Get<bool>("@Success");
            string Message = parameters.Get<string>("@Message");
            string Role = parameters.Get<string>("@Role");
          
            return (Success, Message, Role);
        }
        #endregion
        //#region
        //public async Task<(bool Success, string Message, string Role)> LoginUserAsync(Login login)
        //{
        //    using var connection = new SqlConnection(_connectionString);
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@EmailId", login.EmailId);
        //    parameters.Add("@Password", login.Password); 
        //    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //    parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 256);
        //    parameters.Add("@Role", dbType: DbType.String, direction: ParameterDirection.Output, size: 256);

        //    await connection.ExecuteAsync("LoginUser", parameters, commandType: CommandType.StoredProcedure);

        //    bool success = parameters.Get<bool>("@Success");
        //    string message = parameters.Get<string>("@Message");    
        //    string role = parameters.Get<string>("@Role");

        //    return (success, message, role);
        //}

        //#endregion
        #region  #region Check Existing Admin
        public Users CheckExistingUser(Users users)
        {
            List<Users> list = new List<Users>();
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                list = dbConnection.Query<Users>("sp_ValidateUser", new { users.Name, users.EmailId }, commandType: CommandType.StoredProcedure).ToList();

            }
            return list.FirstOrDefault();
        }
        #endregion
    }
}

