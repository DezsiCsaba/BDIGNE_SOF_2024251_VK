namespace bdigne_api.Db.Models.DataTransferModels;

public class TicketDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public TicketStatus Status { get; set; }
    
    public PriorityLevel Priority { get; set; }

    
    public int AssigneeId { get; set; }

    public int ReporterId { get; set; }

    public int ProjectId { get; set; }
    

    public DateTime DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    
    public TicketDto ConvertFromTicketToDto(Ticket ticket)
    {
        return new TicketDto()
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            Priority = ticket.Priority,
            
            AssigneeId = ticket.AssigneeId,
            ReporterId = ticket.ReporterId,
            ProjectId = ticket.ProjectId,
            
            DueDate = ticket.DueDate,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt
        };
    }
}