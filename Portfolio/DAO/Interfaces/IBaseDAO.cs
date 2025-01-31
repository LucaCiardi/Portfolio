using Microsoft.Data.SqlClient;

namespace Portfolio.DAO.Interfaces
{
    public interface IBaseDAO
    {
        Task<SqlConnection> CreateConnectionAsync();
        Task<bool> TestConnectionAsync();
    }

}
