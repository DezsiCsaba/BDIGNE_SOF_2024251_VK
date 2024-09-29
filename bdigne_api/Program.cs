using System.Text;
using bdigne_api.Db;
using bdigne_api.MiddleWare;
using bdigne_api.Router;
using bdigne_api.Services;
using bdigne_api.Services.AuthService;
using bdigne_api.Services.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

#region configuration
#region base setup, cors, dbContext..
var builder = WebApplication.CreateSlimBuilder(args);

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        // Allow requests from any origin, any method, and any header
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"))
);
#endregion

#region JWT service registration
var jwtSettings = builder.Configuration.GetSection("JWT");
var secretKey = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration[issuer],
            ValidAudience = builder.Configuration[issuer],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey))
        };
    });
#endregion

#region custom service registration
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<Developement>();
builder.Services.AddScoped(typeof(IGenericServiceCrud<>), typeof(Crud<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(NonCrud<>));
builder.Services.AddScoped(typeof(IActivityService<>), typeof(Activity<>));
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
#endregion

#region app & middlewares
var app = builder.Build();

app.UseMiddleware<GlobalErorhandler>();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
// app.UseAuthorization();
#endregion 

#region routers
Test.Map(app);
User.Map(app);
Ticket.Map(app);
Project.Map(app);
#endregion
#endregion

//"deprecated" - moved this functionality to endpoint (Test router)
//could be uncommented for fast and easy testing if needed
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     dbContext.CheckTablesAndRecreateIfNeeded();
//     dbContext.ClearDatabase();
//     dbContext.Seed();
// }

app.Run();

