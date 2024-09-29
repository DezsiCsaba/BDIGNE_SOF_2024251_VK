namespace bdigne_api.Services.AuthService;

public interface IAuthService
{
    Task<Object> AuthenticateAsync(string UserName, string Password); //ez teknikailag deprecated code
    Task<Object> GenerateToken(string UserName);
}