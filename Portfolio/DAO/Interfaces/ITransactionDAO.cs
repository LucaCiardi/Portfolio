using Portfolio.Models;

namespace Portfolio.DAO.Interfaces
{
    public interface ITransactionDAO
    {
        Task<long> AddTransactionAsync(Transaction transaction);
        Task<Transaction> GetTransactionByIdAsync(long transactionId);
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> UpdateTransactionAsync(Transaction transaction);
        Task<bool> DeleteTransactionAsync(long transactionId);
    }
}
