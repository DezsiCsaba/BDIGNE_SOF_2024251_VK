using bdigne_api.Controller;
using bdigne_api.Services;
using Microsoft.AspNetCore.Identity.Data;
using bdigne_api.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace bdigne_api.Router;


public static class Test
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/dummy/auth", Dummy.GetDummyToken );
        

        app.MapPost("/db/recreate/clear/seed", async ([FromQuery] string force, [FromServices]Developement devService) =>
        {
            dynamic done = await devService.FullClearAndSeedDb(force);
            if (done.success == true)
            {
                return Results.Ok();
            }
            return Results.Problem(detail: "Something went wrong while clearing or seeding the database");
        });
        
        app.MapPost("/db/clear/seed", async ([FromServices]Developement devService) =>
        {
            dynamic done = await devService.ClearTablesAndSeedDb();
            if (done.success == true)
            {
                return Results.Ok();
            }
            return Results.Problem(detail: "Something went wrong while clearing or seeding the database");
        });
        
        //ténylegesen használt endpointok:
        app.MapGet("/handshake",  Dummy.HandShake );
    }

}