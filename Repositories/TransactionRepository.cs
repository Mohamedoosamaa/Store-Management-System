using Microsoft.Data.SqlClient;
using StoreManagementSystem.Helpers;
using StoreManagementSystem.Models;

namespace StoreManagementSystem.Repositories
{
    public class TransactionRepository
    {
        // ================= SAVE TRANSACTION =================

        public async Task SaveTransactionAsync(
            Transaction transaction)
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            SqlTransaction sqlTransaction =
                connection.BeginTransaction();

            try
            {
                // ================= INSERT TRANSACTION =================

                string transactionQuery =
                    @"INSERT INTO Transactions
                    (UserId, TransactionDate,
                     SubTotal, VAT,
                     FinalTotal, PaymentMethod)
                    OUTPUT INSERTED.TransactionId
                    VALUES
                    (@UserId, @TransactionDate,
                     @SubTotal, @VAT,
                     @FinalTotal, @PaymentMethod)";

                SqlCommand transactionCommand =
                    new SqlCommand(
                        transactionQuery,
                        connection,
                        sqlTransaction);

                transactionCommand.Parameters.AddWithValue(
                    "@UserId",
                    transaction.UserId);

                transactionCommand.Parameters.AddWithValue(
                    "@TransactionDate",
                    transaction.TransactionDate);

                transactionCommand.Parameters.AddWithValue(
                    "@SubTotal",
                    transaction.SubTotal);

                transactionCommand.Parameters.AddWithValue(
                    "@VAT",
                    transaction.VAT);

                transactionCommand.Parameters.AddWithValue(
                    "@FinalTotal",
                    transaction.FinalTotal);

                transactionCommand.Parameters.AddWithValue(
                    "@PaymentMethod",
                    "Cash");

                int transactionId =
                    (int)await transactionCommand
                    .ExecuteScalarAsync();

                // ================= INSERT ITEMS =================

                foreach (var item in transaction.Items)
                {
                    string itemQuery =
                        @"INSERT INTO TransactionItems
                        (TransactionId, ProductId,
                         Quantity, UnitPrice,
                         LineTotal)
                        VALUES
                        (@TransactionId, @ProductId,
                         @Quantity, @UnitPrice,
                         @LineTotal)";

                    SqlCommand itemCommand =
                        new SqlCommand(
                            itemQuery,
                            connection,
                            sqlTransaction);

                    itemCommand.Parameters.AddWithValue(
                        "@TransactionId",
                        transactionId);

                    itemCommand.Parameters.AddWithValue(
                        "@ProductId",
                        item.ProductId);

                    itemCommand.Parameters.AddWithValue(
                        "@Quantity",
                        item.Quantity);

                    itemCommand.Parameters.AddWithValue(
                        "@UnitPrice",
                        item.UnitPrice);

                    itemCommand.Parameters.AddWithValue(
                        "@LineTotal",
                        item.LineTotal);

                    await itemCommand.ExecuteNonQueryAsync();

                    // ================= UPDATE STOCK =================

                    string stockQuery =
                        @"UPDATE Products
                          SET StockQuantity =
                              StockQuantity - @Quantity
                          WHERE ProductId = @ProductId";

                    SqlCommand stockCommand =
                        new SqlCommand(
                            stockQuery,
                            connection,
                            sqlTransaction);

                    stockCommand.Parameters.AddWithValue(
                        "@Quantity",
                        item.Quantity);

                    stockCommand.Parameters.AddWithValue(
                        "@ProductId",
                        item.ProductId);

                    await stockCommand.ExecuteNonQueryAsync();
                }

                // ================= COMMIT =================

                await sqlTransaction.CommitAsync();
            }
            catch
            {
                await sqlTransaction.RollbackAsync();

                throw;
            }
        }

        // ================= GET ALL TRANSACTIONS =================

        public async Task<List<Transaction>>
            GetAllTransactionsAsync()
        {
            List<Transaction> transactions = new();

            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT *
                  FROM Transactions
                  ORDER BY TransactionDate DESC";

            SqlCommand command =
                new SqlCommand(query, connection);

            SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                transactions.Add(new Transaction
                {
                    TransactionId =
                        (int)reader["TransactionId"],

                    UserId =
                        (int)reader["UserId"],

                    TransactionDate =
                        (DateTime)reader["TransactionDate"]
                });
            }

            return transactions;
        }

        // ================= GET TRANSACTION ITEMS =================

        public async Task<List<TransactionItem>>
            GetTransactionItemsAsync(
                int transactionId)
        {
            List<TransactionItem> items = new();

            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT
                    ti.*,
                    p.Name
                  FROM TransactionItems ti
                  INNER JOIN Products p
                  ON ti.ProductId = p.ProductId
                  WHERE ti.TransactionId = @TransactionId";

            SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@TransactionId",
                transactionId);

            SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                items.Add(new TransactionItem
                {
                    TransactionItemId =
                        (int)reader["TransactionItemId"],

                    TransactionId =
                        (int)reader["TransactionId"],

                    ProductId =
                        (int)reader["ProductId"],

                    ProductName =
                        reader["Name"].ToString()!,

                    Quantity =
                        (int)reader["Quantity"],

                    UnitPrice =
                        (decimal)reader["UnitPrice"]
                });
            }

            return items;
        }
        // ================= TODAY SALES =================

        public async Task<decimal> GetTodaySalesAsync()
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT ISNULL(
            SUM(FinalTotal),0)
          FROM Transactions
          WHERE CAST(TransactionDate AS DATE)
          = CAST(GETDATE() AS DATE)";

            SqlCommand command =
                new SqlCommand(query, connection);

            return Convert.ToDecimal(
                await command.ExecuteScalarAsync());
        }

        // ================= MONTHLY REVENUE =================

        public async Task<decimal> GetMonthlyRevenueAsync()
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT ISNULL(
            SUM(FinalTotal),0)
          FROM Transactions
          WHERE MONTH(TransactionDate)
          = MONTH(GETDATE())
          AND YEAR(TransactionDate)
          = YEAR(GETDATE())";

            SqlCommand command =
                new SqlCommand(query, connection);

            return Convert.ToDecimal(
                await command.ExecuteScalarAsync());
        }
        // ================= TOP SELLING PRODUCTS =================

        public async Task<Dictionary<string, int>>
            GetTopSellingProductsAsync()
        {
            Dictionary<string, int> products = new();

            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT TOP 10
            p.Name,
            SUM(ti.Quantity) AS TotalSold
          FROM TransactionItems ti
          INNER JOIN Products p
          ON ti.ProductId = p.ProductId
          GROUP BY p.Name
          ORDER BY TotalSold DESC";

            SqlCommand command =
                new SqlCommand(query, connection);

            SqlDataReader reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(
                    reader["Name"].ToString()!,
                    Convert.ToInt32(
                        reader["TotalSold"]));
            }

            return products;
        }
        // ================= WEEKLY SALES =================

        public async Task<List<double>>
            GetWeeklySalesAsync()
        {
            List<double> sales = new();

            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            for (int i = 6; i >= 0; i--)
            {
                DateTime day =
                    DateTime.Today.AddDays(-i);

                string query =
                    @"SELECT ISNULL(
                SUM(FinalTotal),0)
              FROM Transactions
              WHERE CAST(TransactionDate AS DATE)
              = @Date";

                SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@Date",
                    day.Date);

                double total =
                    Convert.ToDouble(
                        await command.ExecuteScalarAsync());

                sales.Add(total);
            }

            return sales;
        }

    }
}