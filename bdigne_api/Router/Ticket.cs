using bdigne_api.Services.Db;
using Microsoft.AspNetCore.Mvc;

namespace bdigne_api.Router;

public static class Ticket
{
    public static void Map(WebApplication app)
    {
        app.MapGet("ticket/get",
            async ([FromServices]IGenericServiceCrud<Db.Models.Ticket> crudService) =>
            {
                var result = await Controller.Ticket.GetAllTickets(crudService);
                return Results.Ok(result);

            });
        
        app.MapGet("ticket/get/asignee/{id}",
            async (int id, [FromServices]IGenericService<Db.Models.Ticket> nonCrudService) =>
            {
                var result = await Controller.Ticket.GetTicketOfAsignee(nonCrudService, id);
                return Results.Ok(result);
            });
        
        app.MapGet("ticket/{id}",
            async (int id, [FromServices]IGenericServiceCrud<Db.Models.Ticket> crudService) =>
            {
                var result = await Controller.Ticket.GetTicketById(crudService, id);
                return Results.Ok(result);
            });
        
        app.MapGet("ticket/get/project/{id}",
            async (int id, [FromServices]IGenericService<Db.Models.Ticket> nonCrudService) =>
            {
                var result = await Controller.Ticket.GetTicketsByProjectId(nonCrudService, id);
                return Results.Ok(result);
            });

        app.MapPut("ticket/update",
            async (
                [FromServices] IGenericServiceCrud<Db.Models.Ticket> crudService,
                [FromServices] IGenericServiceCrud<Db.Models.Activity> activityService,
                [FromBody] Db.Models.Activity activity
                ) =>
            {
                await Controller.Ticket.UpdateTicket(crudService, activity, activityService);
                return Results.Ok();
            });

    }
}