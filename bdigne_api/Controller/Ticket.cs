using bdigne_api.Db;
using bdigne_api.Db.Models.DataTransferModels;
using bdigne_api.Services.Db;

namespace bdigne_api.Controller;

public static class Ticket
{
    public static async Task<Object> GetAllTickets(IGenericServiceCrud<Db.Models.Ticket> crudService)
    {
        return new
        {
            tickets = await crudService.GetAllAsync()
        };
    }

    public static async Task<Object> GetTicketOfAsignee(IGenericService<Db.Models.Ticket> nonCrudService, int id)
    {
        return new
        {
            // tickets = await nonCrudService.FindAllWithOption(ticket => ticket.AssigneeId.Equals(id))
            tickets = await nonCrudService.FindAllWithOptionAndInclude(
                t => t.AssigneeId.Equals(id),
                [
                    t => t.Assignee,
                    t => t.Reporter,
                    t => t.Project,
                ]
            )
        };
    }
    
    public static async Task<Object> GetTicketById(IGenericServiceCrud<Db.Models.Ticket> crudService, int id)
    {
        return new
        {
            ticket = await crudService.GetByIdAsync<TicketFirstDto>(
                id,
                [
                    t => t.Assignee,
                    t => t.Reporter,
                    t => t.Project,
                    t => t.Activities
                ]
            )
        };
    }
    
    public static async Task<Object> GetTicketsByProjectId(IGenericService<Db.Models.Ticket> nonCrudService, int id)
    {
        return new
        {
            tickets = await nonCrudService.FindAllWithOption(ticket => ticket.ProjectId.Equals(id))
        };
    }

    public static async Task UpdateTicket(
        IGenericServiceCrud<Db.Models.Ticket> crudService,
        Db.Models.Activity activity,
        IGenericServiceCrud<Db.Models.Activity> activityService)
    {
        activity.Ticket.UpdatedAt = DateTime.Now; //hiába az 'ON UPDATE CURRENT_TIMESTAMP', nem updateli, mert felül van írva a bejövő adatta //TODO - minden updatere kellene, szval a crudService-ben kellene megvalósítani, de ez most egyszerűbb volt
        await crudService.UpdateAsync(activity.Ticket);

        activity.CreatedAt = DateTime.Now;
        await activityService.CreateAsync(activity);
    }
    
}