using System.Net;

namespace bdigne_api.MiddleWare;

public class GlobalErorhandler
{
    private readonly RequestDelegate _next;

    public GlobalErorhandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await HandleException(context, e);
        }
    }

    public async Task HandleException(HttpContext context, Exception exception)
    {
        //TODO - custom logger bevezetése
        Console.WriteLine($"Error occurred: {exception.Message}");
        
        //TODO - custom ApiError bevezetése és kezelése
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        await context.Response.WriteAsJsonAsync(
            new
            {
                success = false,
                message = "Something went wrong..",
                error = exception,
                errorMessage = exception.Message //TODO - should not send this back!!! - we need the custom ApiError
            }
        );
    }
    
}