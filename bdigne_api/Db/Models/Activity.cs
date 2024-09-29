namespace bdigne_api.Db.Models;

public class Activity
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }

    //TODO - commentek
    // public int CommentId { get; set; }
    // public Comment Comment { get; set; }

    public string Event { get; set; }
    public string Type { get; set; }
    public string Original { get; set; }
    public string New { get; set; }

    public DateTime CreatedAt { get; set; }

}

