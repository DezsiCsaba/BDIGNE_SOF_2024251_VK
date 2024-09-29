namespace bdigne_api.Db.Models;

public enum UserRole
{
    Admin,
    Dev,
    ProjectManager,
    SiteDeveloper
}

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } //TODO - hashed pw
    public UserRole Role { get; set; }
    
    public ICollection<Ticket> Tickets {get; set;}
    public ICollection<Ticket> ReportedTickets {get; set;}
    public ICollection<Project> ProjectsCreated {get; set;}
    public ICollection<Activity> Activities {get; set;}
}

