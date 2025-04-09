using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.SignalR
{
    public class NotificationHub :Hub
    {
        // Method that the backend will use to send messages to clients
        public async Task SendMessage(string message)
        {
            // Send message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
