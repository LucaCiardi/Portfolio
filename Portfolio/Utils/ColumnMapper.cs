// ColumnMapper.cs
namespace Portfolio.Utils.Excel
{
    public class ColumnMapper
    {
        private readonly Dictionary<string, string> _mappings;

        public ColumnMapper()
        {
            _mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddMapping(string excelColumn, string databaseField)
        {
            _mappings[excelColumn] = databaseField;
        }

        public string GetDatabaseField(string excelColumn)
        {
            return _mappings.TryGetValue(excelColumn, out string dbField) ? dbField : null;
        }

        public bool HasMapping(string excelColumn)
        {
            return _mappings.ContainsKey(excelColumn);
        }

        public Dictionary<string, string> GetAllMappings()
        {
            return new Dictionary<string, string>(_mappings);
        }

        public static ColumnMapper CreateDefault()
        {
            var mapper = new ColumnMapper();
            mapper.AddMapping("Date", "TransactionDate");
            mapper.AddMapping("Amount", "Amount");
            mapper.AddMapping("Category", "CategoryName");
            mapper.AddMapping("Description", "Description");
            mapper.AddMapping("Type", "TransactionType");
            mapper.AddMapping("Source", "Source");
            return mapper;
        }

        public void Clear()
        {
            _mappings.Clear();
        }

        public bool ValidateRequiredColumns(IEnumerable<string> excelColumns)
        {
            var requiredFields = new[] { "Date", "Amount", "Category", "Description", "Type" };
            return requiredFields.All(field =>
                excelColumns.Any(col => GetDatabaseField(col) == field));
        }

        public List<string> GetMissingRequiredColumns(IEnumerable<string> excelColumns)
        {
            var requiredFields = new[] { "Date", "Amount", "Category", "Description", "Type" };
            return requiredFields
                .Where(field => !excelColumns.Any(col =>
                    GetDatabaseField(col) == field))
                .ToList();
        }
    }
}
