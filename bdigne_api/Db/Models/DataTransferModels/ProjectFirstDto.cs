namespace bdigne_api.Db.Models.DataTransferModels;

public class ProjectFirstDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    
    public int CreatedbyId { get; set; }
    public UserDtO Createdby { get; set; }
    public ICollection<TicketDto> Tickets { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}