using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskManagement.Model.DTOs;
using TaskManagement.Repositiories;
using TaskManagement.SignalR;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly ITrackingRepository trackingRepository;
        private readonly ITrackingState trackingState;
        private readonly IHubContext<NotificationHub> hubContext;

        public TrackingController(ITrackingRepository trackingRepository,ITrackingState trackingState, IHubContext<NotificationHub> hubContext)
        {
            this.trackingRepository = trackingRepository;
            this.trackingState = trackingState;
            this.hubContext = hubContext;
        }

        [HttpPost("addTracking")]
        public async Task<IActionResult> storeCoordinates([FromBody] AddUserCoordinatesDTO details)
        {
            await this.trackingRepository.AddLocationAsync(details);
            return Ok();
        }

        [HttpPost("notify")]
        public async Task<IActionResult> NotifyFrontend()
        {
            //var result = await this.trackingRepository.getCoordinates();
            //// Send message to all connected clients
           await this.hubContext.Clients.All.SendAsync("ReceiveMessage", "New data is available!");

            return Ok();
        }


        [HttpPost("start")]
        public IActionResult StartTracking([FromBody] string email)
        {
            trackingState.SetTrackingId(email);
            return Ok();
        }


      
    }
}
