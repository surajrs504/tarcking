using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskManagement.Model.Domain;
using TaskManagement.Repositiories;
using TaskManagement.SignalR;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingRepository trackingRepository;
        private readonly IHubContext<NotificationHub> hubContext;

        public TrackingController(ITrackingRepository trackingRepository, IHubContext<NotificationHub> hubContext)
        {
            this.trackingRepository = trackingRepository;
            this.hubContext = hubContext;
        }

        [HttpPost("addTracking")]
        public async  Task<IActionResult> storeCoordinates([FromBody] TrackingDomain details)
        {
            await this.trackingRepository.addCooridinates(details);
            return Ok(details);
        }

        [HttpGet("getCoordinates")]
        public async Task<IActionResult> getCoordinates()
        {
            var result = await this.trackingRepository.getCoordinates();
            Console.WriteLine("hello this in cotroller");
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("notify")]
        public async Task<IActionResult> NotifyFrontend()
        {
            var result = await this.trackingRepository.getCoordinates();
            // Send message to all connected clients
            await this.hubContext.Clients.All.SendAsync("ReceiveMessage", "New data is available!");

            return Ok();
        }
    }
}
