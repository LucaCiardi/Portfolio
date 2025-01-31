// ExcelHelper.cs
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Portfolio.Utils.Excel
{
    public static class ExcelHelper
    {
        public static IWorkbook CreateWorkbook()
        {
            return new XSSFWorkbook();
        }

        public static bool IsValidExcelFile(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension == ".xlsx" || extension == ".xls";
        }

        public static string GetCellValueAsString(ICell cell)
        {
            if (cell == null) return string.Empty;

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        var dateValue = cell.DateCellValue;
                        return dateValue.HasValue ? dateValue.Value.ToShortDateString() : string.Empty;
                    }
                    return cell.NumericCellValue.ToString();
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Formula:
                    return cell.CellFormula;
                default:
                    return string.Empty;
            }
        }

        public static void SetCellValue(ICell cell, string value, string dataType)
        {
            if (string.IsNullOrEmpty(value))
            {
                cell.SetCellValue(string.Empty);
                return;
            }

            switch (dataType.ToLower())
            {
                case "date":
                    if (DateTime.TryParse(value, out DateTime dateValue))
                    {
                        cell.SetCellValue(dateValue);
                    }
                    break;
                case "number":
                    if (decimal.TryParse(value, out decimal numValue))
                    {
                        cell.SetCellValue((double)numValue);
                    }
                    break;
                default:
                    cell.SetCellValue(value);
                    break;
            }
        }

        public static void AutoSizeColumns(ISheet sheet, int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }
    }
}
