
using Microsoft.AspNetCore.SignalR;
using TaskManagement.DataContext;
using TaskManagement.Repositiories;
using TaskManagement.SignalR;

namespace TaskManagement.BgServices
{
    public class DataBackgroundService : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<DataBackgroundService> _logger;

        public DataBackgroundService(IHubContext<NotificationHub> hubContext, IServiceScopeFactory serviceScopeFactory,
                                     ILogger<DataBackgroundService> logger)
        {
            _hubContext = hubContext;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Create a scope to resolve scoped services (like DbContext or repositories)
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        // Resolve the scoped service
                        var trackingRepository = scope.ServiceProvider.GetRequiredService<ITrackingRepository>();

                        // Fetch data from the repository
                        var data = await trackingRepository.getLatestCoordinate();

                        _logger.LogInformation($"Sending data: {data}");
                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", data);
                       await Task.Delay(1000, stoppingToken);

                        //// Loop through the fetched data and send it via SignalR to clients
                        //foreach (var item in data)
                        //{
                        //    _logger.LogInformation($"Sending data: {item}");
                        //    await _hubContext.Clients.All.SendAsync("ReceiveMessage", item);
                        //    await Task.Delay(1000, stoppingToken);  // Wait for 1 second before sending the next message
                        //}
                    }

                    // Optionally, delay the entire process to repeat the task after some time
                    //await Task.Delay(5000, stoppingToken);  // Delay for 5 seconds before re-fetching data
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while executing background service");
                }
            }
        }
    
    }
}
