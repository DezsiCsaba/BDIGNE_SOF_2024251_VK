namespace bdigne_api.Db.Models.DataTransferModels;

public class TicketFirstDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketStatus Status { get; set; }
    public PriorityLevel Priority { get; set; }

    
    public int AssigneeId { get; set; }
    public UserDtO Assignee { get; set; }
    
    public int ReporterId { get; set; }
    public UserDtO Reporter { get; set; }

    public int ProjectId { get; set; }
    public ProjectDto Project { get; set; }

    public ICollection<ActivityDto> Activities { get; set; }
    
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}