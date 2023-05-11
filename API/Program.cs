using System.Security.Cryptography;
using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); //uso mi middleware de excepciones

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

//authentication middleware
app.UseAuthentication();  // this ask ¿ you have a valid token?
app.UseAuthorization();   //  you have a valid token, now ¿ what you are allow to do?

app.MapControllers();

using var scope = app.Services.CreateScope(); //nos da acceso a todos los services en el program class.
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();  //obtengo la db
    await context.Database.MigrateAsync();  //aplica migraciones pendientes de la db, y sino existe la db la crea
    await Seed.SeedUsers(context); //llamo a mi clase seed para insertar todos los usuarios en db
}
catch (Exception ex)
{

    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration of users");
}

app.Run();
