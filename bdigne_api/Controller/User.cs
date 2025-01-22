using bdigne_api.Services;
using bdigne_api.Db.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using bdigne_api.Db.Models.DataTransferModels;
using bdigne_api.Services.AuthService;
using bdigne_api.Services.Db;
using Microsoft.AspNetCore.Mvc;

namespace bdigne_api.Controller;

public static class User
{
    public static async Task<UserDtO> AuthUser(string username, string password, IGenericService<Db.Models.User> userService)
    {
        dynamic user = await userService.FindWithOptions(user => 
            user.UserName.Equals(username) &&
            user.Password.Equals(password));

        if (user != null)
        {
            UserDtO userDto = new UserDtO().ConvertFromUserToDtO(user);
            return userDto;
        }

        return user;
    }

    public static async Task RegisterUser(
        Db.Models.User user,
        IGenericServiceCrud<Db.Models.User> userService)
    {
        await userService.CreateAsync(user);
    }

    public static async Task<Object> GetAll(IGenericServiceCrud<Db.Models.User> crudService)
    {
        return new
        {
            users = await crudService.GetAllAsync()
        };
    }
}