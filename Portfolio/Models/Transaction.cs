namespace Portfolio.Models
{
    public class Transaction
    {
        public long TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string TransactionType { get; set; }

        public string Source { get; set; }

        public DateTime CreatedDate { get; set; }
    }

}
