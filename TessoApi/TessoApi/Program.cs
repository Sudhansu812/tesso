using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TessoApi.Helpers;
using TessoApi.Helpers.Interfaces;
using TessoApi.Helpers.Middlewares;
using TessoApi.Models.Identity;
using TessoApi.Repository;
using TessoApi.Repository.DB;
using TessoApi.Repository.Interfaces;
using TessoApi.Services;
using TessoApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Appsettings Setup
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile($"{Directory.GetCurrentDirectory()}\\Configurations\\appsettings.json");
#endregion

#region Database Context
string appDb = builder.Configuration.GetConnectionString("appDb") ?? throw new InvalidOperationException($"ConnectionsString {nameof(appDb)} not found.");
string authDb = builder.Configuration.GetConnectionString("authDb") ?? throw new InvalidOperationException($"ConnectionsString {nameof(authDb)} not found.");
string exceptionDb = builder.Configuration.GetConnectionString("exceptionDb") ?? throw new InvalidOperationException($"ConnectionsString {nameof(exceptionDb)} not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appDb));
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(authDb));
builder.Services.AddDbContext<ExceptionDbContext>(options => options.UseNpgsql(authDb));
#endregion

#region Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region DI
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IUserServices, UserServices>();
builder.Services.AddTransient<ICollectionService, CollectionService>();

builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<ICollectionRepository, CollectionRepository>();

builder.Services.AddSingleton<IObjectValidationHelper, ObjectValidationHelper>();
#endregion

#region Authentication
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateLifetime = true
    };
});

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

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
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
