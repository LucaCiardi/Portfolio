using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using Portfolio.DAO.Interfaces;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITransactionDAO _transactionDAO;
        private readonly ICategoryDAO _categoryDAO;
        private readonly IReportDAO _reportDAO;

        public HomeController(
            ITransactionDAO transactionDAO,
            ICategoryDAO categoryDAO,
            IReportDAO reportDAO)
        {
            _transactionDAO = transactionDAO;
            _categoryDAO = categoryDAO;
            _reportDAO = reportDAO;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var startDate = today.AddMonths(-1);

            var viewModel = new DashboardViewModel
            {
                RecentTransactions = await _transactionDAO.GetTransactionsByDateRangeAsync(startDate, today),
                TopSpendingCategories = await _reportDAO.GetTopSpendingCategoriesAsync(startDate, today),
                IncomeVsExpenses = await _reportDAO.GetIncomeVsExpensesAsync(startDate, today)
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
