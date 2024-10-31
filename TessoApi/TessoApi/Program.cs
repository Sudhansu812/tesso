using Microsoft.EntityFrameworkCore;
using TessoApi.Repository.DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Appsettings setup
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile($"./Configurations/appsettings.json");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile($"./Configurations/appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
#endregion

#region
string appDb = builder.Configuration.GetConnectionString("appDb") ?? throw new InvalidOperationException($"ConnectionsString {nameof(appDb)} not found.");
string authDb = builder.Configuration.GetConnectionString("authDb") ?? throw new InvalidOperationException($"ConnectionsString {nameof(authDb)} not found.");
string exceptionDb = builder.Configuration.GetConnectionString("AppDb") ?? throw new InvalidOperationException($"ConnectionsString {nameof(exceptionDb)} not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appDb));
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(authDb));
builder.Services.AddDbContext<ExceptionDbContext>(options => options.UseNpgsql(authDb));
#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
