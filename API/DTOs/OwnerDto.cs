namespace CView.API.DTOs;

public class OwnerDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateOwnerDto
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}

public class UpdateOwnerDto
{
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
