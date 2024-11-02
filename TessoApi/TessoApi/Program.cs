using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TessoApi.Models.Identity;
using TessoApi.Repository.DB;
using TessoApi.Services;
using TessoApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Appsettings Setup
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

#region DI
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
#endregion

#region Authentication
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = "/Auth/Login";
});
#endregion

#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT Auth API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer prefix",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
#endregion

builder.Services.AddControllers();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
