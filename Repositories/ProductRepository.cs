using Microsoft.Data.SqlClient;
using StoreManagementSystem.Helpers;
using StoreManagementSystem.Models;

namespace StoreManagementSystem.Repositories
{
    public class ProductRepository
    {
        // ================= GET ALL PRODUCTS =================

        public async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> products = new();

            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT *
                  FROM Products
                  WHERE IsActive = 1";

            SqlCommand command =
                new SqlCommand(query, connection);

            SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    Id =
                        (int)reader["ProductId"],

                    Name =
                        reader["Name"].ToString()!,

                    SKU =
                        reader["SKU"].ToString()!,

                    Price =
                        (decimal)reader["Price"],

                    StockQuantity =
                        (int)reader["StockQuantity"],

                    Category =
                        reader["Category"].ToString()!
                });
            }

            return products;
        }

        // ================= ADD PRODUCT =================

        public async Task AddProductAsync(Product product)
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"INSERT INTO Products
                  (Name, SKU, Price,
                   StockQuantity, Category)
                  VALUES
                  (@Name, @SKU, @Price,
                   @StockQuantity, @Category)";

            SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@Name",
                product.Name);

            command.Parameters.AddWithValue(
                "@SKU",
                product.SKU);

            command.Parameters.AddWithValue(
                "@Price",
                product.Price);

            command.Parameters.AddWithValue(
                "@StockQuantity",
                product.StockQuantity);

            command.Parameters.AddWithValue(
                "@Category",
                product.Category);

            await command.ExecuteNonQueryAsync();
        }

        // ================= UPDATE PRODUCT =================

        public async Task UpdateProductAsync(Product product)
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"UPDATE Products
                  SET Name = @Name,
                      SKU = @SKU,
                      Price = @Price,
                      StockQuantity = @StockQuantity,
                      Category = @Category
                  WHERE ProductId = @Id";

            SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@Id",
                product.Id);

            command.Parameters.AddWithValue(
                "@Name",
                product.Name);

            command.Parameters.AddWithValue(
                "@SKU",
                product.SKU);

            command.Parameters.AddWithValue(
                "@Price",
                product.Price);

            command.Parameters.AddWithValue(
                "@StockQuantity",
                product.StockQuantity);

            command.Parameters.AddWithValue(
                "@Category",
                product.Category);

            await command.ExecuteNonQueryAsync();
        }

        // ================= SOFT DELETE PRODUCT =================

        public async Task DeleteProductAsync(int productId)
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"UPDATE Products
                  SET IsActive = 0
                  WHERE ProductId = @ProductId";

            SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@ProductId",
                productId);

            await command.ExecuteNonQueryAsync();
        }

        // ================= UPDATE STOCK =================

        public async Task UpdateStockAsync(
            int productId,
            int quantity)
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"UPDATE Products
                  SET StockQuantity =
                      StockQuantity - @Quantity
                  WHERE ProductId = @ProductId";

            SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@Quantity",
                quantity);

            command.Parameters.AddWithValue(
                "@ProductId",
                productId);

            await command.ExecuteNonQueryAsync();
        }
        // ================= PRODUCTS COUNT =================

        public async Task<int> GetProductsCountAsync()
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT COUNT(*)
          FROM Products
          WHERE IsActive = 1";

            SqlCommand command =
                new SqlCommand(query, connection);

            return (int)await command.ExecuteScalarAsync();
        }

        // ================= LOW STOCK COUNT =================

        public async Task<int> GetLowStockCountAsync()
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT COUNT(*)
          FROM Products
          WHERE StockQuantity <= 5
          AND IsActive = 1";

            SqlCommand command =
                new SqlCommand(query, connection);

            return (int)await command.ExecuteScalarAsync();
        }

        // ================= IN STOCK COUNT =================

        public async Task<int> GetInStockCountAsync()
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT COUNT(*)
          FROM Products
          WHERE StockQuantity > 10
          AND IsActive = 1";

            SqlCommand command =
                new SqlCommand(query, connection);

            return (int)await command.ExecuteScalarAsync();
        }

        // ================= OUT OF STOCK COUNT =================

        public async Task<int> GetOutOfStockCountAsync()
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT COUNT(*)
          FROM Products
          WHERE StockQuantity = 0
          AND IsActive = 1";

            SqlCommand command =
                new SqlCommand(query, connection);

            return (int)await command.ExecuteScalarAsync();
        }



    }
}