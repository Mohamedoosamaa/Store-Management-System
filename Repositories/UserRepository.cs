using Microsoft.Data.SqlClient;
using StoreManagementSystem.Helpers;
using StoreManagementSystem.Models;

namespace StoreManagementSystem.Repositories
{
    public class UserRepository
    {
        // ================= LOGIN =================

        public async Task<User?> LoginAsync(
            string username,
            string password)
        {
            using SqlConnection connection =
                DbConnection.GetConnection();

            await connection.OpenAsync();

            string query =
                @"SELECT *
                  FROM Users
                  WHERE Username = @Username
                  AND PasswordHash = @Password
                  AND IsActive = 1";

            SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@Username",
                username);

            command.Parameters.AddWithValue(
                "@Password",
                password);

            SqlDataReader reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId =
                        (int)reader["UserId"],

                    Username =
                        reader["Username"].ToString()!,

                    PasswordHash =
                        reader["PasswordHash"].ToString()!,

                    FullName =
                        reader["FullName"].ToString()!,

                    Role =
                        reader["Role"].ToString()!,

                    IsActive =
                        (bool)reader["IsActive"]
                };
            }

            return null;
        }
    }
}