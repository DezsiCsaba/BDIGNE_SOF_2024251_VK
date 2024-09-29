namespace bdigne_api.Db.Models.DataTransferModels;

public class ActivityDto
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserDtO User { get; set; }

    public int TicketId { get; set; }
    public TicketDto Ticket { get; set; }

    //TODO - commentek
    // public int CommentId { get; set; }
    // public Comment Comment { get; set; }

    public string Event { get; set; } //pl -> {User} updated {Ticket} status from open to done, v ilyesmi ez majd eldől h pontosan h lesz
    public string Type { get; set; }
    public string Original { get; set; }
    public string New { get; set; }

    public DateTime CreatedAt { get; set; }


    public ActivityDto ConvertFromActivityToDto(Activity activity)
    {
        return new ActivityDto
        {
            Id = activity.Id,
            UserId = activity.UserId,
            User = new UserDtO().ConvertFromUserToDtO(activity.User),
            TicketId = activity.TicketId,
            Event = activity.Event,
            Type = activity.Type,
            Original = activity.Original,
            New = activity.New,
            CreatedAt = activity.CreatedAt,
        };
    }
}