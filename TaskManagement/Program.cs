using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagement.BgServices;
using TaskManagement.DataContext;
using TaskManagement.Repositiories;
using TaskManagement.SignalR;   

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<TaskManagmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
builder.Services.AddDbContext<TaskManagementAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnectionString")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITrackingRepository, TrackingRepository>();
builder.Services.AddScoped<ITokenInterface, TokenRepository>();

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<TaskManagementAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    //deafult validation settings for password
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequiredLength = 6;
});


//this config it to setup the token validation in the app
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });


builder.Services.AddOpenApi();

builder.Services.AddSignalR();
builder.Services.AddTransient<IHostedService, DataBackgroundService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:50492")  // Allow specific origin (Angular frontend URL)
               .AllowAnyMethod()                    // Allow any HTTP method (GET, POST, etc.)
               .AllowAnyHeader()                    // Allow any header
               .AllowCredentials();
        builder.WithOrigins("http://localhost:4200")  // Allow specific origin (Angular frontend URL)
               .AllowAnyMethod()                    // Allow any HTTP method (GET, POST, etc.)
               .AllowAnyHeader()                    // Allow any header
               .AllowCredentials();  // Allow credentials (cookies, HTTP headers, etc.)
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
 
}
app.UseCors("AllowAngularFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAngularFrontend");

app.MapControllers();

app.Run();
