namespace Portfolio.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<Transaction> RecentTransactions { get; set; }
        public IEnumerable<TopSpendingCategory> TopSpendingCategories { get; set; }
        public IEnumerable<IncomeVsExpense> IncomeVsExpenses { get; set; }
    }

}
