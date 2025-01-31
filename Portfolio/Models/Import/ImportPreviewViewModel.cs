namespace Portfolio.Models.Import
{
    public class ImportPreviewViewModel
    {
        public List<TransactionPreview> Transactions { get; set; }
        public Dictionary<string, string> ColumnMappings { get; set; }
        public string ImportId { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }
    }

}
