using bdigne_api.Db.Models.DataTransferModels;
using bdigne_api.Services.Db;

namespace bdigne_api.Controller;

public static class Project
{
    public static async Task<Object> GetAllProjects(IGenericServiceCrud<Db.Models.Project> crudService)
    {
        return new
        {
            projects = await crudService.GetAllAsync()
        };
    }

    public static async Task<Object> GetProjectById(IGenericServiceCrud<Db.Models.Project> crudService, int id)
    {
        return new
        {
            project = await crudService.GetByIdAsync<ProjectFirstDto>(
                id,
                [
                    p => p.Tickets,
                    p => p.Createdby
                ]
            )
        };
    }
}