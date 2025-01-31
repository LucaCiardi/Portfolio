namespace Portfolio.Models
{
    public class MonthlySummary
    {
        public string CategoryType { get; set; }
        public string CategoryName { get; set; }
        public int TransactionCount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AverageAmount { get; set; }
    }

    public class DateRangeSummary
    {
        public string CategoryType { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
    }

    public class DailyBalance
    {
        public DateTime TransactionDate { get; set; }
        public decimal Balance { get; set; }
    }

    public class CategoryTrend
    {
        public string CategoryName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
    }

    public class TopSpendingCategory
    {
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
    }

    public class IncomeVsExpense
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetAmount { get; set; }
    }

}
