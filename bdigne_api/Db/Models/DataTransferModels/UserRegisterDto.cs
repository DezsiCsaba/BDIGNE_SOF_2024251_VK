namespace bdigne_api.Db.Models.DataTransferModels;

public class UserRegisterDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }


    public User GetUserObject(UserRegisterDto userRegisterDto)
    {
        return new User
        {
            UserName = userRegisterDto.UserName,
            Email = userRegisterDto.Email,
            Password = userRegisterDto.Password,
            Role = GetUserRole(userRegisterDto.Role)
        };
    }

    private UserRole GetUserRole(string input)
    {
        if (Enum.TryParse<UserRole>(input, true, out var role))
        {
            return role;
        }
        return UserRole.Dev;
    }
}