using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class CategoryViewModel
    {
        [Required]
        [Display(Name = "Category Name")]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Category Type")]
        public string CategoryType { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // For dropdown lists in forms
        public IEnumerable<SelectListItem> CategoryTypes { get; set; }
    }

}
