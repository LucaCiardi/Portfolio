using Microsoft.Data.SqlClient;
using Portfolio.DAO.Interfaces;
using Portfolio.Models;
using System.Data;

namespace Portfolio.DAO.Implementations
{
    public class CategoryDAO : BaseDAO, ICategoryDAO
    {
        public CategoryDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> AddCategoryAsync(Category category)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_AddCategory", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            command.Parameters.AddWithValue("@CategoryType", category.CategoryType);
            command.Parameters.AddWithValue("@Description", category.Description);

            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetCategoryByID", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@CategoryID", categoryId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Category
                {
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    CategoryType = reader.GetString(reader.GetOrdinal("CategoryType")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                };
            }
            return null;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = new List<Category>();
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetAllCategories", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    CategoryType = reader.GetString(reader.GetOrdinal("CategoryType")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                });
            }
            return categories;
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithStatsAsync()
        {
            var categories = new List<Category>();
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_GetCategoriesWithCount", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                    CategoryType = reader.GetString(reader.GetOrdinal("CategoryType")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    TransactionCount = reader.GetInt32(reader.GetOrdinal("TransactionCount")),
                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
                });
            }
            return categories;
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_UpdateCategory", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@CategoryID", category.CategoryId);
            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            command.Parameters.AddWithValue("@CategoryType", category.CategoryType);
            command.Parameters.AddWithValue("@Description", category.Description);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            using var connection = await CreateConnectionAsync();
            using var command = new SqlCommand("sp_DeleteCategory", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@CategoryID", categoryId);
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) == 1;
        }
    }

}
