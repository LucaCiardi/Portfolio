using Microsoft.Data.SqlClient;
using Portfolio.DAO.Interfaces;

namespace Portfolio.DAO.Implementations
{
    public abstract class BaseDAO : IBaseDAO
    {
        private readonly string _connectionString;

        protected BaseDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<SqlConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = await CreateConnectionAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
