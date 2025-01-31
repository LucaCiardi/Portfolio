namespace Portfolio.Models.Import
{
    public class ColumnMapping
    {
        public string ExcelColumn { get; set; }
        public string DatabaseField { get; set; }
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }

        public static Dictionary<string, ColumnMapping> GetDefaultMappings()
        {
            return new Dictionary<string, ColumnMapping>
            {
                { "TransactionDate", new ColumnMapping
                    {
                        DatabaseField = "TransactionDate",
                        IsRequired = true,
                        DefaultValue = null
                    }
                },
                { "Amount", new ColumnMapping
                    {
                        DatabaseField = "Amount",
                        IsRequired = true,
                        DefaultValue = null
                    }
                },
                { "Category", new ColumnMapping
                    {
                        DatabaseField = "CategoryName",
                        IsRequired = true,
                        DefaultValue = null
                    }
                },
                { "Description", new ColumnMapping
                    {
                        DatabaseField = "Description",
                        IsRequired = true,
                        DefaultValue = null
                    }
                },
                { "Type", new ColumnMapping
                    {
                        DatabaseField = "TransactionType",
                        IsRequired = true,
                        DefaultValue = "Expense"
                    }
                },
                { "Source", new ColumnMapping
                    {
                        DatabaseField = "Source",
                        IsRequired = false,
                        DefaultValue = "Imported"
                    }
                }
            };
        }
    }
}
