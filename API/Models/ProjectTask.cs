namespace CView.API.Models;

public class ProjectTask : BaseEntity
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OwnerId { get; set; }
    public DateTime PlannedStartsAt { get; set; }
    public DateTime PlannedEndsAt { get; set; }
    public DateTime? ActualStartsAt { get; set; }
    public DateTime? ActualEndsAt { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.NotSet;

    // Navigation properties
    public Project Project { get; set; } = null!;
    public Owner? Owner { get; set; }
}
