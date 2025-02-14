using ShoppingCart.Interface;
using ShoppingCart.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using NuGet.Protocol.Plugins;
namespace ShoppingCart.Services
{
    public class ProductServices:IProductServices
    {
        private readonly string? _connectionString;

        public ProductServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }
        public async Task InsertOrUpdateProductAsync(Product product)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var parameters = new
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.PricePerQnt,
                    Quantity = product.AvailableQnty,
                    ProductImage = product.ProductImage,    
                    Description = product.Description
                };

                await dbConnection.ExecuteAsync("USPInsertUpdateProduct", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        //public async Task<Product> GetProductById(int id)
        //{
        //    using (IDbConnection dbConnection = new SqlConnection(_connectionString))
        //    {
        //         //dbConnection.Open();
        //        //var sql = "SELECT * FROM Product WHERE ProductId = @Id";
        //        //var product = await dbConnection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        //        //return product;
        //        dbConnection.Open();
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@id", id);
        //        var result = await dbConnection.QueryFirstOrDefaultAsync<Product>("exec USPGetProductById", parameters, commandType: CommandType.StoredProcedure);
        //        return result;
        //    }
        //}
        public async Task<Product?> GetProductByIdAsync(int Id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);
                return await dbConnection.QuerySingleOrDefaultAsync<Product>(
                    "USPGetProductById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
          using (var dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var sql = "SELECT ProductId, Name as ProductName,Price as PricePerQnt ,Quantity as AvailableQnty,SoldQnt,ProductImage,Description,CreatedDatetime,ModifiedDateTime FROM Product where Isdeleted=0 and Status=1";
                return await dbConnection.QueryAsync<Product>(sql);
               
            }
        }
               
        public async Task AddToCartProductAsync(int id)
        {

            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {

                dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                await dbConnection.ExecuteAsync("USPAddToCartProduct", parameters, commandType: CommandType.StoredProcedure);

            }


        }

        //public async Task<bool> UpdateProduct(Product product)
        //{
        //    using (var dbConnection = Connection)
        //    {
        //        var sql = @"UPDATE Products 
        //                SET Name = @Name, Price = @Price, Quantity = @Quantity, Description = @Description, ImageFileName = @ImageFileName 
        //                WHERE Id = @Id";

        //        var affectedRows = await dbConnection.ExecuteAsync(sql, product);
        //        return affectedRows > 0;
        //    }
        //}

        //public async Task<bool> DeleteProduct(int id)
        //{
        //    using (var dbConnection = new SqlConnection(_connectionString))
        //    {
        //        var sql = "DELETE FROM Products WHERE Id = @Id";
        //        var affectedRows = await dbConnection.ExecuteAsync(sql, new { Id = id });
        //        return affectedRows > 0;
        //    }
        //}
        public async Task<bool> DeleteProductAsync(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                var affectedRows= await dbConnection.ExecuteAsync("USpDeleteProduct", parameters, commandType: CommandType.StoredProcedure);
                
                return affectedRows > 0;
            }
        }

        #region Deactivate Product
        public async Task InActivateProductAsync(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {

                dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                await dbConnection.ExecuteAsync("USPActiveInactiveProduct", parameters, commandType: CommandType.StoredProcedure);

            }

        }
        #endregion
        #region Activate Product
        public async Task ActivateProductAsync(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {

                dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                await dbConnection.ExecuteAsync("USPActiveInactiveProduct", parameters, commandType: CommandType.StoredProcedure);

            }

        }
        #endregion

    }
}
