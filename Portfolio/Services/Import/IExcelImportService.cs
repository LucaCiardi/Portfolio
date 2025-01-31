// IExcelImportService.cs
using Portfolio.Models.Import;

namespace Portfolio.Services.Import
{
    public interface IExcelImportService
    {
        Task<ImportPreviewViewModel> GetPreviewDataAsync(IFormFile file);
        Task<ImportResult> ProcessImportAsync(string importId, Dictionary<string, string> columnMappings);
        Task<bool> ValidateFileAsync(IFormFile file);
    }
}
