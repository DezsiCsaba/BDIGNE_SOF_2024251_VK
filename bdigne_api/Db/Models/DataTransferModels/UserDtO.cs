namespace bdigne_api.Db.Models.DataTransferModels;

public class UserDtO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public int UserRoleInt { get; set; }


    public UserDtO ConvertFromUserToDtO(User user)
    {
        return new UserDtO
        {
            //a jelszót nem akarjuk visszaadni (obviously)
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role,
            UserRoleInt = (int)user.Role
        };
    }
}
