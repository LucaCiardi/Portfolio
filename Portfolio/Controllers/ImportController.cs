using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Portfolio.Models.Import;
using Portfolio.Services.Import;
using Portfolio.DAO.Interfaces;

namespace Portfolio.Controllers
{
    public class ImportController : Controller
    {
        private readonly IExcelImportService _importService;
        private readonly ITransactionDAO _transactionDAO;
        private readonly ICategoryDAO _categoryDAO;

        public ImportController(
            IExcelImportService importService,
            ITransactionDAO transactionDAO,
            ICategoryDAO categoryDAO)
        {
            _importService = importService;
            _transactionDAO = transactionDAO;
            _categoryDAO = categoryDAO;
        }

        public IActionResult Index()
        {
            return View(new ImportViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(ImportViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            try
            {
                if (model.ExcelFile == null || model.ExcelFile.Length == 0)
                {
                    ModelState.AddModelError("", "Please select a file to upload.");
                    return View("Index", model);
                }

                // Validate file extension
                var extension = Path.GetExtension(model.ExcelFile.FileName).ToLowerInvariant();
                if (extension != ".xlsx" && extension != ".xls")
                {
                    ModelState.AddModelError("", "Please upload a valid Excel file (.xlsx or .xls)");
                    return View("Index", model);
                }

                // Process the Excel file and get preview data
                var previewData = await _importService.GetPreviewDataAsync(model.ExcelFile);

                // Store preview data in TempData or Session for the next step
                TempData["ImportId"] = previewData.ImportId;

                return View("Preview", previewData);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error processing file: {ex.Message}");
                return View("Index", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(string importId, Dictionary<string, string> columnMappings)
        {
            try
            {
                if (string.IsNullOrEmpty(importId))
                {
                    return RedirectToAction("Index");
                }

                // Process the import using the confirmed mappings
                var result = await _importService.ProcessImportAsync(importId, columnMappings);

                // Save transactions to database
                foreach (var transaction in result.ValidTransactions)
                {
                    await _transactionDAO.AddTransactionAsync(transaction);
                }

                // Prepare result summary
                var summary = new ImportResultViewModel
                {
                    TotalRows = result.TotalRows,
                    SuccessfulImports = result.ValidTransactions.Count,
                    FailedImports = result.InvalidTransactions.Count,
                    Errors = result.Errors
                };

                return View("Result", summary);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Import failed: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult DownloadTemplate()
        {
            try
            {
                // Create new workbook
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Transactions");

                // Create header row
                IRow headerRow = sheet.CreateRow(0);
                var headers = new[] { "Date", "Amount", "Category", "Description", "Type", "Source" };

                for (int i = 0; i < headers.Length; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(headers[i]);
                }

                // Write to MemoryStream
                using (var ms = new MemoryStream())
                {
                    workbook.Write(ms, true);
                    var fileBytes = ms.ToArray();

                    return File(
                        fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "TransactionImportTemplate.xlsx");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error generating template: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ValidateCategory(string categoryName)
        {
            var categories = await _categoryDAO.GetAllCategoriesAsync();
            var exists = categories.Any(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            return Json(new { exists });
        }
    }
}
