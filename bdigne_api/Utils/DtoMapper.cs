using bdigne_api.Db.Models;
using bdigne_api.Db.Models.DataTransferModels;

namespace bdigne_api.Utils;

public class DtoMapper
{
    public TDto MapToDto<T, TDto>(T entity) where T : class
    {
        if (typeof(T) == typeof(Ticket))
        {
            var ticket = entity as Ticket;
            return (TDto)(object)new TicketFirstDto
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
                UpdatedAt = ticket.UpdatedAt,
                
                Assignee = new UserDtO().ConvertFromUserToDtO(ticket.Assignee),
                Reporter = new UserDtO().ConvertFromUserToDtO(ticket.Reporter),
                Project = new ProjectDto().ConvertFromProjectToDto(ticket.Project),
                Activities = ticket.Activities.Select(a => new ActivityDto().ConvertFromActivityToDto(a)).ToList(),
            };
        }
        else if (typeof(T) == typeof(Project))
        {
            var project = entity as Project;
            return (TDto)(object)new ProjectFirstDto
            {
                Id = project.Id,
                Name = project.Name,
                ShortName = project.ShortName,
                Description = project.Description,

                CreatedbyId = project.CreatedbyId,
                Createdby = new UserDtO().ConvertFromUserToDtO(project.Createdby),
                Tickets = project.Tickets?.Select(t => new TicketDto().ConvertFromTicketToDto(t)).ToList(),

                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
            };
        }

        //TODO - majd userFirstDto mapolása
        
        throw new NotImplementedException($"Mapping not implemented for {typeof(T).Name}");
    }
}