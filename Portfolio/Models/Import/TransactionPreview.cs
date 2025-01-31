namespace Portfolio.Models.Import
{
    public class TransactionPreview
    {
        public int RowNumber { get; set; }
        public Dictionary<string, string> RawData { get; set; }
        public bool IsValid { get; set; }
        public List<string> ValidationErrors { get; set; }

        // Mapped fields
        public DateTime? TransactionDate { get; set; }
        public decimal? Amount { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public string Source { get; set; }

        public TransactionPreview()
        {
            RawData = new Dictionary<string, string>();
            ValidationErrors = new List<string>();
            IsValid = true;
        }
    }
}
