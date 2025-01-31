namespace Portfolio.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryType { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation property
        public int TransactionCount { get; set; }

        public decimal TotalAmount { get; set; }
    }

}
