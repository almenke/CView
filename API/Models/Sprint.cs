namespace CView.API.Models;

public class Sprint : BaseEntity
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }

    // Navigation properties
    public Project Project { get; set; } = null!;
}
