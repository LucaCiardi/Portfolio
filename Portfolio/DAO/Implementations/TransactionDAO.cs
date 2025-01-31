using Microsoft.Data.SqlClient;
using Portfolio.DAO.Interfaces;
using System.Data;
using Portfolio.Models;

namespace Portfolio.DAO.Implementations
{
    public class TransactionDAO : BaseDAO, ITransactionDAO
    {
        public TransactionDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<long> AddTransactionAsync(Transaction transaction)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_AddTransaction", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
            command.Parameters.AddWithValue("@Amount", transaction.Amount);
            command.Parameters.AddWithValue("@CategoryID", transaction.CategoryId);
            command.Parameters.AddWithValue("@Description", transaction.Description);
            command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
            command.Parameters.AddWithValue("@Source", transaction.Source);

            return Convert.ToInt64(await command.ExecuteScalarAsync());
        }

        public async Task<Transaction> GetTransactionByIdAsync(long transactionId)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetTransactionByID", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TransactionID", transactionId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Transaction
                {
                    TransactionId = reader.GetInt64(reader.GetOrdinal("TransactionID")),
                    TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    TransactionType = reader.GetString(reader.GetOrdinal("TransactionType")),
                    Source = reader.GetString(reader.GetOrdinal("Source")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                };
            }
            return null;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var transactions = new List<Transaction>();
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetTransactionsByDateRange", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                transactions.Add(new Transaction
                {
                    TransactionId = reader.GetInt64(reader.GetOrdinal("TransactionID")),
                    TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    TransactionType = reader.GetString(reader.GetOrdinal("TransactionType")),
                    Source = reader.GetString(reader.GetOrdinal("Source")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                });
            }
            return transactions;
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_UpdateTransaction", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TransactionID", transaction.TransactionId);
            command.Parameters.AddWithValue("@TransactionDate", transaction.TransactionDate);
            command.Parameters.AddWithValue("@Amount", transaction.Amount);
            command.Parameters.AddWithValue("@CategoryID", transaction.CategoryId);
            command.Parameters.AddWithValue("@Description", transaction.Description);
            command.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
            command.Parameters.AddWithValue("@Source", transaction.Source);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteTransactionAsync(long transactionId)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_DeleteTransaction", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@TransactionID", transactionId);
            return await command.ExecuteNonQueryAsync() > 0;
        }
    }

}
