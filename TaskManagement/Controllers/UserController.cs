using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Model.DTOs;
using TaskManagement.Repositiories;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRespository userRepository;

        public UserController(IUserRespository userRespository)
        {
            this.userRepository = userRespository;
            
        }

        [HttpPost("approveUser")]
        public async Task<IActionResult> ApproveUser([FromBody] UserStatusChangeDTO payload)
        {
            var result= await this.userRepository.ApproveUser(payload.Email);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("rejectUser")]
        public async Task<IActionResult> RejectUser([FromBody] UserStatusChangeDTO payload)
        {
            var result = await this.userRepository.RejectUser(payload.Email);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("changeTrackingStatus")]
        public async Task<IActionResult> ChangeTrackingStatus([FromBody] UserTrackingStatusChangeDTO payload)
        {
            var result = await this.userRepository.ChangeTrackingStatus(payload);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("getTrackingUser")]
        public async Task<IActionResult> GetTrackingUserList()
        {
            var result =  await this.userRepository.GetTrackingUserList();
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
