using bdigne_api.Db.Models.DataTransferModels;

namespace bdigne_api.Db.Models;

//TODO - megoldani a kötöttséget a statuszok között, illetve kibővíteni ilyenekkel mint pl testing...

public enum TicketStatus
{
    Open, InProgress, Done, Closed
}

public enum PriorityLevel
{
    Low, Medium, High
}

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketStatus Status { get; set; }
    public PriorityLevel Priority { get; set; }
    
    public int AssigneeId { get; set; }
    public User Assignee { get; set; }

    public int ReporterId { get; set; }
    public User Reporter { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public ICollection<Activity> Activities  { get; set; }

    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}