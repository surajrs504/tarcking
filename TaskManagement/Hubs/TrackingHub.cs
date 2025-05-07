using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskManagement.Hubs
{
    public class TrackingHub:Hub
    {
        private static readonly ConcurrentDictionary<string, string> _connections = new();

        public Task SubscribeToLocation(string email)
        {
            _connections[Context.ConnectionId] = email;  // Overwrite the previous email for the same connection
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connections.TryRemove(Context.ConnectionId, out _);
            return base.OnDisconnectedAsync(exception);
        }

        // Helper: Get current subscribed emails (used by BackgroundService)
        public static IReadOnlyCollection<string> SubscribedEmails =>
            _connections.Values.Distinct().ToList();

        // Helper: Get connectionIds for a specific email
        public static List<string> GetConnectionIdsByEmail(string email)
        {
            return _connections
                .Where(pair => pair.Value == email)
                .Select(pair => pair.Key)
                .ToList();
        }

        public Task UnsubscribeFromLocation()
        {
            _connections.TryRemove(Context.ConnectionId, out _);
            return Task.CompletedTask;
        }
       
    }
}
