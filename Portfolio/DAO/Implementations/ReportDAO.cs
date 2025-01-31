using Microsoft.Data.SqlClient;
using Portfolio.DAO.Interfaces;
using Portfolio.Models;
using System.Data;

namespace Portfolio.DAO.Implementations
{
    public class ReportDAO : BaseDAO, IReportDAO
    {
        public ReportDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<MonthlySummary>> GetMonthlySummaryAsync(int year, int month)
        {
            var summaries = new List<MonthlySummary>();
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetMonthlySummary", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Year", year);
            command.Parameters.AddWithValue("@Month", month);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                summaries.Add(new MonthlySummary
                {
                    CategoryType = reader.GetString(reader.GetOrdinal("CategoryType")),
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    TransactionCount = reader.GetInt32(reader.GetOrdinal("TransactionCount")),
                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                    AverageAmount = reader.GetDecimal(reader.GetOrdinal("AverageAmount"))
                });
            }
            return summaries;
        }

        public async Task<IEnumerable<DateRangeSummary>> GetDateRangeSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var summaries = new List<DateRangeSummary>();
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetDateRangeSummary", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                summaries.Add(new DateRangeSummary
                {
                    CategoryType = reader.GetString(reader.GetOrdinal("CategoryType")),
                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                    TransactionCount = reader.GetInt32(reader.GetOrdinal("TransactionCount"))
                });
            }
            return summaries;
        }

        public async Task<IEnumerable<DailyBalance>> GetDailyBalanceAsync(DateTime startDate, DateTime endDate)
        {
            var balances = new List<DailyBalance>();
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetDailyBalance", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                balances.Add(new DailyBalance
                {
                    TransactionDate = reader.GetDateTime(reader.GetOrdinal("TransactionDate")),
                    Balance = reader.GetDecimal(reader.GetOrdinal("DailyBalance"))
                });
            }
            return balances;
        }

        public async Task<IEnumerable<CategoryTrend>> GetCategoryTrendsAsync(DateTime startDate, DateTime endDate)
        {
            var trends = new List<CategoryTrend>();
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetCategoryTrends", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                trends.Add(new CategoryTrend
                {
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    Month = reader.GetInt32(reader.GetOrdinal("Month")),
                    Year = reader.GetInt32(reader.GetOrdinal("Year")),
                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                    TransactionCount = reader.GetInt32(reader.GetOrdinal("TransactionCount"))
                });
            }
            return trends;
        }

        public async Task<IEnumerable<TopSpendingCategory>> GetTopSpendingCategoriesAsync(
            DateTime startDate, DateTime endDate, int topCount = 5)
        {
            var categories = new List<TopSpendingCategory>();
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetTopSpendingCategories", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);
            command.Parameters.AddWithValue("@TopCount", topCount);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                categories.Add(new TopSpendingCategory
                {
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                    TransactionCount = reader.GetInt32(reader.GetOrdinal("TransactionCount"))
                });
            }
            return categories;
        }

        public async Task<IEnumerable<IncomeVsExpense>> GetIncomeVsExpensesAsync(DateTime startDate, DateTime endDate)
        {
            var results = new List<IncomeVsExpense>();
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetIncomeVsExpenses", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(new IncomeVsExpense
                {
                    Month = reader.GetInt32(reader.GetOrdinal("Month")),
                    Year = reader.GetInt32(reader.GetOrdinal("Year")),
                    TotalIncome = reader.GetDecimal(reader.GetOrdinal("TotalIncome")),
                    TotalExpenses = reader.GetDecimal(reader.GetOrdinal("TotalExpenses")),
                    NetAmount = reader.GetDecimal(reader.GetOrdinal("NetAmount"))
                });
            }
            return results;
        }
    }

}
