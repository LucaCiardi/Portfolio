using Portfolio.Models;

namespace Portfolio.DAO.Interfaces
{
    public interface IReportDAO
    {
        Task<IEnumerable<MonthlySummary>> GetMonthlySummaryAsync(int year, int month);
        Task<IEnumerable<DateRangeSummary>> GetDateRangeSummaryAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DailyBalance>> GetDailyBalanceAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<CategoryTrend>> GetCategoryTrendsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<TopSpendingCategory>> GetTopSpendingCategoriesAsync(DateTime startDate, DateTime endDate, int topCount = 5);
        Task<IEnumerable<IncomeVsExpense>> GetIncomeVsExpensesAsync(DateTime startDate, DateTime endDate);
    }

}
