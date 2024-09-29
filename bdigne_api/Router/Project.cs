using bdigne_api.Services.Db;
using Microsoft.AspNetCore.Mvc;

namespace bdigne_api.Router;

public static class Project
{
    public static void Map(WebApplication appp)
    {
        appp.MapGet("project", async ([FromServices]IGenericServiceCrud<Db.Models.Project> crudService) =>
        {
            var result = await Controller.Project.GetAllProjects(crudService);
            return Results.Ok(result);
        });

        appp.MapGet("project/{id}", async (int id, [FromServices]IGenericServiceCrud<Db.Models.Project> crudService) =>
        {
            var result = await Controller.Project.GetProjectById(crudService, id);
            return Results.Ok(result);
        });
    }
}