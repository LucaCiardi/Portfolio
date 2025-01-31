using Portfolio.Models;
using Portfolio.DAO.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Portfolio.Services.Import
{
    public class ImportValidationService
    {
        private readonly ICategoryDAO _categoryDAO;

        public ImportValidationService(ICategoryDAO categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }

        public async Task<ValidationResult> ValidateTransactionAsync(Transaction transaction)
        {
            var result = new ValidationResult();

            // Validate Date
            if (transaction.TransactionDate == default)
                result.Errors.Add("Invalid transaction date");

            // Validate Amount
            if (transaction.Amount == 0)
                result.Errors.Add("Amount cannot be zero");

            // Validate Category
            var categories = await _categoryDAO.GetAllCategoriesAsync();
            if (!categories.Any(c => c.CategoryName.Equals(transaction.CategoryName, StringComparison.OrdinalIgnoreCase)))
                result.Errors.Add($"Category '{transaction.CategoryName}' does not exist");

            // Validate Description
            if (string.IsNullOrWhiteSpace(transaction.Description))
                result.Errors.Add("Description is required");

            // Validate Transaction Type
            if (!new[] { "Income", "Expense" }.Contains(transaction.TransactionType, StringComparer.OrdinalIgnoreCase))
                result.Errors.Add("Invalid transaction type. Must be 'Income' or 'Expense'");

            result.IsValid = !result.Errors.Any();
            return result;
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class ImportResult
    {
        public List<Transaction> ValidTransactions { get; set; } = new List<Transaction>();
        public List<Transaction> InvalidTransactions { get; set; } = new List<Transaction>();
        public List<string> Errors { get; set; } = new List<string>();
        public int TotalRows => ValidTransactions.Count + InvalidTransactions.Count;
    }
}
