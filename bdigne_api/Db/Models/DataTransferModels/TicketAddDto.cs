namespace bdigne_api.Db.Models.DataTransferModels;

public class TicketAddDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Status { get; set; }

    public string Priority { get; set; }


    public int AssigneeId { get; set; }

    public int ReporterId { get; set; }

    public int ProjectId { get; set; }


    public string DueDate { get; set; }

    public string CreatedAt { get; set; }

    public string UpdatedAt { get; set; }

    // public DateTime CreatedAt { get; set; } = DateTime.Now;
    //
    // public DateTime UpdatedAt { get; set; } = DateTime.Now;


    public Ticket GetTicketFromDto(TicketAddDto ticketDto)
    {
        return new Ticket()
        {
            Id = ticketDto.Id,
            Title = ticketDto.Title,
            Description = ticketDto.Description,
            Status = GetTicketStatus(ticketDto.Status),
            Priority = GetPriorityLevel(ticketDto.Priority),

            AssigneeId = ticketDto.AssigneeId,
            ReporterId = ticketDto.ReporterId,
            ProjectId = ticketDto.ProjectId,

            DueDate = DateTime.Now,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    private TicketStatus GetTicketStatus(string input)
    {
        if (Enum.TryParse<TicketStatus>(input, true, out var status))
        {
            return status;
        }
        return TicketStatus.Open;
    }

    private PriorityLevel GetPriorityLevel(string input)
    {
        if (Enum.TryParse<PriorityLevel>(input, true, out var prio))
        {
            return prio;
        }
        return PriorityLevel.Low;
    }
}