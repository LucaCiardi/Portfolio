namespace Portfolio.Models.Import
{
    public class ImportViewModel
    {
        public IFormFile ExcelFile { get; set; }
        public Dictionary<string, string> ColumnMappings { get; set; }
    }

}
