using Microsoft.AspNetCore.Mvc;
using Portfolio.DAO.Interfaces;

namespace Portfolio.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportDAO _reportDAO;

        public ReportController(IReportDAO reportDAO)
        {
            _reportDAO = reportDAO;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> MonthlySummary(int year, int month)
        {
            var summary = await _reportDAO.GetMonthlySummaryAsync(year, month);
            return View(summary);
        }

        public async Task<IActionResult> CategoryTrends()
        {
            var startDate = DateTime.Today.AddMonths(-6);
            var trends = await _reportDAO.GetCategoryTrendsAsync(startDate, DateTime.Today);
            return View(trends);
        }

        public async Task<IActionResult> IncomeVsExpenses()
        {
            var startDate = DateTime.Today.AddMonths(-12);
            var data = await _reportDAO.GetIncomeVsExpensesAsync(startDate, DateTime.Today);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> TopSpendingCategories(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today.AddMonths(-1);
            endDate ??= DateTime.Today;

            var categories = await _reportDAO.GetTopSpendingCategoriesAsync(startDate.Value, endDate.Value);
            return View(categories);
        }

        [HttpGet]
        public async Task<JsonResult> GetDailyBalanceData(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today.AddMonths(-1);
            endDate ??= DateTime.Today;

            var balances = await _reportDAO.GetDailyBalanceAsync(startDate.Value, endDate.Value);
            return Json(balances);
        }
    }

}
