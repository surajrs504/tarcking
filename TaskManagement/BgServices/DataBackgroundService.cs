
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DataContext;
using TaskManagement.Hubs;
using TaskManagement.Repositiories;
using TaskManagement.SignalR;

namespace TaskManagement.BgServices
{
    public class DataBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _provider;

        private readonly IHubContext<TrackingHub> _hubContext;

        private readonly ILogger<DataBackgroundService> _logger;

        private readonly Dictionary<string, double> _lastSent = new();
        public DataBackgroundService(IServiceProvider provider, IHubContext<TrackingHub> hubContext, ILogger<DataBackgroundService> logger)
        {
            _provider = provider;
            _hubContext = hubContext;
            _logger = logger;


        }

        //private readonly IHubContext<SignalR.NotificationHub> _hubContext;
        //private readonly ITrackingState trackingState;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        //private readonly ILogger<DataBackgroundService> _logger;

        //public DataBackgroundService(IHubContext<SignalR.NotificationHub> hubContext,ITrackingState trackingState,  IServiceScopeFactory serviceScopeFactory,
        //                             ILogger<DataBackgroundService> logger)
        //{
        //    _hubContext = hubContext;
        //    this.trackingState = trackingState;
        //    _serviceScopeFactory = serviceScopeFactory;
        //    _logger = logger;
        //}

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("Background Service started.");

        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        try

        //        {
        //            var connectionIds = LocationHub.GetConnectionIdsByEmail(email);
        //            await _hubContext.Clients.Clients(connectionIds)
        //                .SendAsync("ReceiveLocation", locationPayload);
        //            var emailId = trackingState.GetTrackingId();

        //            // Create a scope to resolve scoped services (like DbContext or repositories)
        //            using (var scope = _serviceScopeFactory.CreateScope())
        //            {
        //                // Resolve the scoped service
        //                var trackingRepository = scope.ServiceProvider.GetRequiredService<ITrackingRepository>();

        //                // Fetch data from the repository
        //                var data = await trackingRepository.GetLocationsByUserIdAsync(emailId);

        //                //_logger.LogInformation($"Sending data: {data}");
        //                //await _hubContext.Clients.All.SendAsync("ReceiveMessage", data);
        //                await Task.Delay(1000, stoppingToken);

        //            }

        //            // Optionally, delay the entire process to repeat the task after some time
        //            //await Task.Delay(5000, stoppingToken);  // Delay for 5 seconds before re-fetching data
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error while executing background service");
        //        }
        //    }
        //}
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Service started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                var subscribedEmails = TrackingHub.SubscribedEmails;

                if (subscribedEmails.Count > 0)
                {
                    using var scope = _provider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    
                    foreach (var email in subscribedEmails)
                    {
                        Console.WriteLine($"emaill received {subscribedEmails}");
                        Console.WriteLine($"emaill received {subscribedEmails}");
                        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
                        if (user == null) continue;
                        Console.WriteLine($"user got{user}");
                        var latestLocation = await db.UserLocations
                            .Where(l => l.UserId == user.Id)
                            .OrderByDescending(l => l.TimeStampMs)
                            .FirstOrDefaultAsync();
                        Console.WriteLine($"latestLocation {latestLocation}");
                      //  if (latestLocation != null &&
                        //    (!_lastSent.ContainsKey(email) || latestLocation.TimeStampMs > _lastSent[email]))
                            if (latestLocation != null)
                            {
                            var connectionIds = TrackingHub.GetConnectionIdsByEmail(email);
                            Console.WriteLine($"latestLocation not null {latestLocation}");
                            await _hubContext.Clients
                                .Clients(connectionIds)
                                .SendAsync("ReceiveLocation", new
                                {
                                    Latitude = latestLocation.Latitude,
                                    Longitude = latestLocation.Longitude,
                                    TimestampMs = latestLocation.TimeStampMs,
                                    Email=email
                                });
                            Console.WriteLine($"latestLocation.TimeStampMs {latestLocation.TimeStampMs}");
                            _lastSent[email] = latestLocation.TimeStampMs;
                        }
                    }
                }

                await Task.Delay(2000, stoppingToken);
            }
        }
        

        private static List<string> GetConnectionIdsByEmail(string email)
        {
            return TrackingHub
                .SubscribedEmails
                .Where(e => e == email)
                .SelectMany(e => TrackingHub
                    .SubscribedEmails
                    .Where(x => x == e))
                .ToList();
        }
    }
}
