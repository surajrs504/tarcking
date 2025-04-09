using Microsoft.EntityFrameworkCore;
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

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITrackingRepository, TrackingRepository>();

builder.Services.AddOpenApi();

builder.Services.AddSignalR();
builder.Services.AddTransient<IHostedService, DataBackgroundService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:58600")  // Allow specific origin (Angular frontend URL)
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

app.UseAuthorization();

app.MapHub<NotificationHub>("/notificationHub").RequireCors("AllowAngularFrontend");
;

app.MapControllers();

app.Run();
