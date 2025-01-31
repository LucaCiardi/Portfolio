using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class TransactionViewModel
    {
        public long TransactionId { get; set; }
        [Required]
        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }

        [Required]
        [StringLength(100)]
        public string Source { get; set; }

        // For dropdown lists in forms
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> TransactionTypes { get; set; }
    }

}
