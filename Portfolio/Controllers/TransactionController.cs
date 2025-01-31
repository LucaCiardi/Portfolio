using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portfolio.DAO.Interfaces;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionDAO _transactionDAO;
        private readonly ICategoryDAO _categoryDAO;

        public TransactionController(
            ITransactionDAO transactionDAO,
            ICategoryDAO categoryDAO)
        {
            _transactionDAO = transactionDAO;
            _categoryDAO = categoryDAO;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionDAO.GetTransactionsByDateRangeAsync(
                DateTime.Today.AddMonths(-1),
                DateTime.Today);
            return View(transactions);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryDAO.GetAllCategoriesAsync();
            var viewModel = new TransactionViewModel
            {
                TransactionDate = DateTime.Today,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    TransactionDate = viewModel.TransactionDate,
                    Amount = viewModel.Amount,
                    CategoryId = viewModel.CategoryId,
                    Description = viewModel.Description,
                    TransactionType = viewModel.TransactionType,
                    Source = viewModel.Source
                };

                await _transactionDAO.AddTransactionAsync(transaction);
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            viewModel.Categories = (await _categoryDAO.GetAllCategoriesAsync())
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                });
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(long id)
        {
            var transaction = await _transactionDAO.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            var categories = await _categoryDAO.GetAllCategoriesAsync();
            var viewModel = new TransactionViewModel
            {
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount,
                CategoryId = transaction.CategoryId,
                Description = transaction.Description,
                TransactionType = transaction.TransactionType,
                Source = transaction.Source,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, TransactionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    TransactionId = id,
                    TransactionDate = viewModel.TransactionDate,
                    Amount = viewModel.Amount,
                    CategoryId = viewModel.CategoryId,
                    Description = viewModel.Description,
                    TransactionType = viewModel.TransactionType,
                    Source = viewModel.Source
                };

                await _transactionDAO.UpdateTransactionAsync(transaction);
                return RedirectToAction(nameof(Index));
            }

            viewModel.Categories = (await _categoryDAO.GetAllCategoriesAsync())
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                });
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            await _transactionDAO.DeleteTransactionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
