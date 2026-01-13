using CView.API.DTOs;

namespace CView.API.Services;

public interface IExcelImportService
{
    Task<ImportResultDto> ImportExcelAsync(int projectId, Stream fileStream);
}

public class ImportResultDto
{
    public int TasksImported { get; set; }
    public int OwnersImported { get; set; }
    public List<string> Errors { get; set; } = new();
    public bool Success => Errors.Count == 0;
}
