using System.Text;
using bdigne_api.Services.AuthService;

namespace bdigne_api.Controller;

public static class Dummy
{
    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        StringBuilder stringBuilder = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(chars[random.Next(chars.Length)]);
        }

        return stringBuilder.ToString();
    }
    
    public static async Task GetDummyToken(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var dummyToken = GenerateRandomString(25);
        await context.Response.WriteAsJsonAsync(new { token = dummyToken });
    }

    public static async Task HandShake(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { date = DateTime.Now });
    }
    
}