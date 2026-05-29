using Microsoft.Data.SqlClient;

namespace StoreManagementSystem.Helpers
{
    public static class DbConnection
    {
        private static readonly string connectionString =
        @"Server=.\SQLEXPRESS;
Database=store_Management;
Trusted_Connection=True;
TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}