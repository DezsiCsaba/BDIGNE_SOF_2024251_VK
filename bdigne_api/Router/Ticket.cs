using bdigne_api.Db.Models;
using bdigne_api.Db.Models.DataTransferModels;
using bdigne_api.MiddleWare;
using bdigne_api.Services.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bdigne_api.Router;

public static class Ticket
{
    [Authorize]
    public static void Map(WebApplication app)
    {
        app.MapGet("ticket/get",
            async ([FromServices]IGenericServiceCrud<Db.Models.Ticket> crudService) =>
            {
                var result = await Controller.Ticket.GetAllTickets(crudService);
                return Results.Ok(result);

            }).RequireAuthorization();
        
        app.MapGet("ticket/get/asignee/{id}",
            async (int id, [FromServices]IGenericService<Db.Models.Ticket> nonCrudService) =>
            {
                var result = await Controller.Ticket.GetTicketOfAsignee(nonCrudService, id);
                return Results.Ok(result);
            }).RequireAuthorization();
        
        app.MapGet("ticket/{id}",
            async (int id, [FromServices]IGenericServiceCrud<Db.Models.Ticket> crudService) =>
            {
                var result = await Controller.Ticket.GetTicketById(crudService, id);
                return Results.Ok(result);
            }).RequireAuthorization();
        
        app.MapGet("ticket/get/project/{id}",
            async (int id, [FromServices]IGenericService<Db.Models.Ticket> nonCrudService) =>
            {
                var result = await Controller.Ticket.GetTicketsByProjectId(nonCrudService, id);
                return Results.Ok(result);
            }).RequireAuthorization();

        app.MapPut("ticket/update",
            async (
                HttpContext httpContext,
                [FromServices] IGenericServiceCrud<Db.Models.Ticket> crudService,
                [FromServices] IGenericServiceCrud<Db.Models.Activity> activityService,
                [FromBody] Db.Models.Activity activity
                ) =>
            {
                if (httpContext.Request.Headers.TryGetValue("X-User-Role", out var userRole))
                {
                    if (userRole != "Dev")
                    {
                        return Results.Unauthorized();
                    }
                }
                // if (activity.User.Role != UserRole.Admin && activity.User.Role != UserRole.Dev)
                // {
                //     return Results.Unauthorized();
                // }
                await Controller.Ticket.UpdateTicket(crudService, activity, activityService);
                return Results.Ok();
            }).RequireAuthorization();

        app.MapPost("ticket/create",
            async (
                HttpContext httpContext,
                [FromServices] IGenericServiceCrud<Db.Models.Ticket> ticketService,
                [FromBody] TicketAddDto ticketAddDto
            ) =>
            {
                if (httpContext.Request.Headers.TryGetValue("X-User-Role", out var userRole))
                {
                    if (userRole != "ProjectManager" && userRole != "Admin")
                    {
                        return Results.Unauthorized();
                    }
                }
                await Controller.Ticket.CreateTicket(ticketAddDto.GetTicketFromDto(ticketAddDto), ticketService);
                return Results.Ok();
            }).RequireAuthorization();

    }
}