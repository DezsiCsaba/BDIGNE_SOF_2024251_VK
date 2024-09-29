namespace bdigne_api.Db.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    
    public int CreatedbyId { get; set; }
    public User Createdby { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}