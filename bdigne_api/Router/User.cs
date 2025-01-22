using bdigne_api.Services.AuthService;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using bdigne_api.Controller;
using bdigne_api.Db.Models.DataTransferModels;
using bdigne_api.Services.Db;

namespace bdigne_api.Router;

public static class User
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/auth", async ([FromBody]LoginRequest request, [FromServices]IAuthService authService, [FromServices]IGenericService<Db.Models.User> nonCrudService) =>
        {
            // dynamic result = await authService.AuthenticateAsync(request.Email, request.Password);
            // if (result.token == null)
            // {
            //     return Results.Unauthorized();
            // }
            // return Results.Ok(result);
            UserDtO user = await Controller.User.AuthUser(request.Email, request.Password, nonCrudService);
            if (user != null)
            {
                var jwt = await authService.GenerateToken(user.UserName);
                return Results.Ok(new
                {
                    token = jwt,
                    userData = user
                });
            }

            return Results.Unauthorized();
        });

        app.MapPost("/register",
            async (
                HttpContext httpContext,
                [FromServices] IGenericServiceCrud<Db.Models.User> userService,
                [FromBody] UserRegisterDto userReg
            ) =>
            {
                if (httpContext.Request.Headers.TryGetValue("X-User-Role", out var userRole))
                {
                    if (userRole != "Admin" && userRole != "SiteDeveloper")
                    {
                        return Results.Unauthorized();
                    }
                }
                await Controller.User.RegisterUser(userReg.GetUserObject(userReg), userService);
                return Results.Ok();
            }).RequireAuthorization();

        app.MapGet("users/get/all",
            async (
                HttpContext httpContext,
                [FromServices] IGenericServiceCrud<Db.Models.User> userService
            ) =>
            {
                if (httpContext.Request.Headers.TryGetValue("X-User-Role", out var userRole))
                {
                    if (userRole != "Admin" && userRole != "SiteDeveloper")
                    {
                        return Results.Unauthorized();
                    }
                }

                var result = await Controller.User.GetAll(userService);
                return Results.Ok(result);
            }).RequireAuthorization();
    }
}