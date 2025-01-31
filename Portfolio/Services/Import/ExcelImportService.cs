using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Portfolio.Models;
using Portfolio.Models.Import;
using Portfolio.DAO.Interfaces;


namespace Portfolio.Services.Import
{
    public class ExcelImportService : IExcelImportService
    {
        private readonly ImportValidationService _validationService;
        private readonly IConfiguration _configuration;
        private readonly string _tempStoragePath;

        public ExcelImportService(
            ImportValidationService validationService,
            IConfiguration configuration)
        {
            _validationService = validationService;
            _configuration = configuration;
            _tempStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "TempImport");

            if (!Directory.Exists(_tempStoragePath))
                Directory.CreateDirectory(_tempStoragePath);
        }

        public async Task<ImportPreviewViewModel> GetPreviewDataAsync(IFormFile file)
        {
            var importId = Guid.NewGuid().ToString();
            var previewModel = new ImportPreviewViewModel
            {
                ImportId = importId,
                Transactions = new List<TransactionPreview>(),
                ColumnMappings = new Dictionary<string, string>()
            };

            using (var stream = file.OpenReadStream())
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(0);

                // Get headers
                var headerRow = sheet.GetRow(0);
                var headers = new List<string>();
                for (int i = 0; i < headerRow.LastCellNum; i++)
                {
                    var cell = headerRow.GetCell(i);
                    headers.Add(cell?.ToString() ?? $"Column{i + 1}");
                }

                // Process rows for preview
                for (int i = 1; i <= Math.Min(sheet.LastRowNum, 5); i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;

                    var preview = new TransactionPreview
                    {
                        RowNumber = i
                    };

                    for (int j = 0; j < headers.Count; j++)
                    {
                        var cell = row.GetCell(j);
                        preview.RawData[headers[j]] = cell?.ToString() ?? string.Empty;
                    }

                    previewModel.Transactions.Add(preview);
                }

                // Save the file temporarily
                var tempFilePath = Path.Combine(_tempStoragePath, $"{importId}.xlsx");
                using (var fs = new FileStream(tempFilePath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }
            }

            return previewModel;
        }

        public async Task<ImportResult> ProcessImportAsync(string importId, Dictionary<string, string> columnMappings)
        {
            var result = new ImportResult();
            var filePath = Path.Combine(_tempStoragePath, $"{importId}.xlsx");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Import file not found");

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);
                    ISheet sheet = workbook.GetSheetAt(0);

                    // Process all rows
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null) continue;

                        var transaction = MapRowToTransaction(row, columnMappings);
                        var validationResult = await _validationService.ValidateTransactionAsync(transaction);

                        if (validationResult.IsValid)
                            result.ValidTransactions.Add(transaction);
                        else
                        {
                            result.InvalidTransactions.Add(transaction);
                            result.Errors.Add($"Row {i}: {string.Join(", ", validationResult.Errors)}");
                        }
                    }
                }
            }
            finally
            {
                // Cleanup
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            return result;
        }

        public async Task<bool> ValidateFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".xlsx" && extension != ".xls")
                return false;

            return true;
        }

        private Transaction MapRowToTransaction(IRow row, Dictionary<string, string> columnMappings)
        {
            var transaction = new Transaction();
            foreach (var mapping in columnMappings)
            {
                var cellIndex = GetColumnIndex(row, mapping.Key);
                if (cellIndex == -1) continue;

                var cell = row.GetCell(cellIndex);
                var value = GetCellValue(cell);

                switch (mapping.Value)
                {
                    case "TransactionDate":
                        transaction.TransactionDate = DateTime.Parse(value);
                        break;
                    case "Amount":
                        transaction.Amount = decimal.Parse(value);
                        break;
                    case "CategoryName":
                        transaction.CategoryName = value;
                        break;
                    case "Description":
                        transaction.Description = value;
                        break;
                    case "TransactionType":
                        transaction.TransactionType = value;
                        break;
                    case "Source":
                        transaction.Source = value;
                        break;
                }
            }
            return transaction;
        }

        private int GetColumnIndex(IRow row, string columnName)
        {
            for (int i = 0; i < row.LastCellNum; i++)
            {
                var cell = row.GetCell(i);
                if (cell?.ToString() == columnName)
                    return i;
            }
            return -1;
        }

        private string GetCellValue(ICell cell)
        {
            if (cell == null) return string.Empty;

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue.HasValue
                            ? cell.DateCellValue.Value.ToShortDateString()
                            : string.Empty;
                    }
                    return cell.NumericCellValue.ToString();
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Formula:
                    return cell.CellFormula;
                default:
                    return string.Empty;
            }
        }
    }
}
