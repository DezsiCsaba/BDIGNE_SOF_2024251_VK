namespace bdigne_api.Db.Models.DataTransferModels;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    
    public int CreatedbyId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ProjectDto ConvertFromProjectToDto(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            ShortName = project.ShortName,
            Description = project.Description,
            CreatedbyId = project.CreatedbyId,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }
}