using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using bdigne_api.Db;
using bdigne_api.Db.Models;
using bdigne_api.Services.Db;
using Microsoft.IdentityModel.Tokens;
using bdigne_api.Db.Models.DataTransferModels;

namespace bdigne_api.Services.AuthService;

public class AuthService: IAuthService
{
    private readonly string _secretKey;
    private readonly ApplicationDbContext _dbContext;
    private readonly IGenericService<bdigne_api.Db.Models.User> _userService;

    public AuthService(IConfiguration configuration, ApplicationDbContext dbContext, IGenericService<bdigne_api.Db.Models.User> userService)
    {
        // _secretKey = configuration["AppSettings:JWT:Key"];
        var jwtSettings = configuration.GetSection("JWT");
        _secretKey = jwtSettings["Key"];
        _dbContext = dbContext;
        _userService = userService;
    }
    


    public async Task<Object> GenerateToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var jwt = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(jwt);
    }
    
    
    /// <summary>
    /// DEPRECATED CODE!!! - used to handle the authentication process
    /// </summary>
    /// <param name="UserName"></param>
    /// <param name="Password"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<Object> AuthenticateAsync(string UserName, string Password)
    {
        dynamic user = await _userService.FindWithOptions(user => 
            user.UserName.Equals(UserName) &&
            user.Password.Equals(Password));
        
        // Check if user exists
        if (user != null)
        {
            // convert to DtO type object, so we get the Role as string and int
            UserDtO userDto = new UserDtO().ConvertFromUserToDtO(user);
            
            #region JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            #endregion
            
            return new
            {
                token = tokenHandler.WriteToken(token),
                userData = new
                {
                    userDto.Id,
                    userDto.UserName,
                    userDto.Role,
                    userDto.UserRoleInt,
                    userDto.Email
                }
            };
        }
        throw new Exception("Auth error");
    }
}