using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

//authentication middleware
app.UseAuthentication();  // this ask ¿ you have a valid token?
app.UseAuthorization();   //  you have a valid token, now ¿ what you are allow to do?

app.MapControllers();

app.Run();
