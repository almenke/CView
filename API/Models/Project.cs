namespace CView.API.Models;

public class Project : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OwnerId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }

    // Navigation properties
    public Owner? Owner { get; set; }
    public ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();
    public ICollection<Owner> Owners { get; set; } = new List<Owner>();
    public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
}
