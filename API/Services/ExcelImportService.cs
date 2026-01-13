using OfficeOpenXml;
using CView.API.Models;
using CView.API.Repositories;

namespace CView.API.Services;

public class ExcelImportService : IExcelImportService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IProjectRepository _projectRepository;

    public ExcelImportService(
        ITaskRepository taskRepository,
        IOwnerRepository ownerRepository,
        IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _ownerRepository = ownerRepository;
        _projectRepository = projectRepository;
    }

    public async Task<ImportResultDto> ImportExcelAsync(int projectId, Stream fileStream)
    {
        var result = new ImportResultDto();

        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null)
        {
            result.Errors.Add("Project not found");
            return result;
        }

        try
        {
            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                result.Errors.Add("No worksheet found in the Excel file");
                return result;
            }

            // Get header row to find column indices
            var headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int col = 1; col <= worksheet.Dimension?.End.Column; col++)
            {
                var header = worksheet.Cells[1, col].Text?.Trim();
                if (!string.IsNullOrEmpty(header))
                {
                    headers[header] = col;
                }
            }

            // Map of owner names to owner entities
            var existingOwners = (await _ownerRepository.GetOwnersByProjectIdAsync(projectId))
                .ToDictionary(o => o.Name, o => o, StringComparer.OrdinalIgnoreCase);

            // Process rows (starting from row 2, assuming row 1 is headers)
            for (int row = 2; row <= worksheet.Dimension?.End.Row; row++)
            {
                try
                {
                    var outlineLevel = GetCellValue(worksheet, row, headers, "Outline Level", "OutlineLevel", "Level");

                    // Only import tasks with decimal outline levels (subtasks)
                    if (!ShouldImportTask(outlineLevel))
                    {
                        continue;
                    }

                    var taskName = GetCellValue(worksheet, row, headers, "Name", "Task Name", "TaskName", "Title");
                    if (string.IsNullOrWhiteSpace(taskName))
                    {
                        continue;
                    }

                    var startDateStr = GetCellValue(worksheet, row, headers, "Start", "Start Date", "StartDate", "PlannedStart");
                    var endDateStr = GetCellValue(worksheet, row, headers, "Finish", "End Date", "EndDate", "End", "PlannedEnd");

                    if (!TryParseDate(worksheet, row, headers, startDateStr, out var startDate, "Start", "Start Date", "StartDate", "PlannedStart"))
                    {
                        result.Errors.Add($"Row {row}: Invalid start date");
                        continue;
                    }

                    if (!TryParseDate(worksheet, row, headers, endDateStr, out var endDate, "Finish", "End Date", "EndDate", "End", "PlannedEnd"))
                    {
                        result.Errors.Add($"Row {row}: Invalid end date");
                        continue;
                    }

                    // Handle owner
                    var ownerName = GetCellValue(worksheet, row, headers, "Resource Names", "Owner", "Assigned To", "AssignedTo");
                    Owner? owner = null;

                    if (!string.IsNullOrWhiteSpace(ownerName))
                    {
                        if (!existingOwners.TryGetValue(ownerName, out owner))
                        {
                            // Create new owner
                            owner = new Owner
                            {
                                ProjectId = projectId,
                                Name = ownerName,
                                Title = string.Empty
                            };
                            await _ownerRepository.AddAsync(owner);
                            existingOwners[ownerName] = owner;
                            result.OwnersImported++;
                        }
                    }

                    // Create task
                    var task = new ProjectTask
                    {
                        ProjectId = projectId,
                        Name = taskName,
                        OwnerId = owner?.Id,
                        PlannedStartsAt = startDate,
                        PlannedEndsAt = endDate,
                        Status = StatusEnum.NotSet
                    };

                    await _taskRepository.AddAsync(task);
                    result.TasksImported++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Row {row}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            result.Errors.Add($"Error processing Excel file: {ex.Message}");
        }

        return result;
    }

    private static bool ShouldImportTask(string? outlineLevel)
    {
        if (string.IsNullOrWhiteSpace(outlineLevel))
        {
            return true; // Import all if no outline level column
        }

        // Check if outline level contains a decimal (indicating a subtask)
        return outlineLevel.Contains('.');
    }

    private static string? GetCellValue(ExcelWorksheet worksheet, int row, Dictionary<string, int> headers, params string[] possibleNames)
    {
        foreach (var name in possibleNames)
        {
            if (headers.TryGetValue(name, out var col))
            {
                var value = worksheet.Cells[row, col].Text?.Trim();
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }
        }
        return null;
    }

    private static bool TryParseDate(ExcelWorksheet worksheet, int row, Dictionary<string, int> headers, string? dateStr, out DateTime date, params string[] possibleNames)
    {
        date = DateTime.MinValue;

        // First try to parse the string value
        if (!string.IsNullOrWhiteSpace(dateStr) && DateTime.TryParse(dateStr, out date))
        {
            return true;
        }

        // Try to get date from cell value directly (Excel stores dates as numbers)
        foreach (var name in possibleNames)
        {
            if (headers.TryGetValue(name, out var col))
            {
                var cell = worksheet.Cells[row, col];
                if (cell.Value is DateTime dt)
                {
                    date = dt;
                    return true;
                }
                if (cell.Value is double d)
                {
                    date = DateTime.FromOADate(d);
                    return true;
                }
            }
        }

        return false;
    }
}
