using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Model.Domain;
using TaskManagement.Repositiories;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository teamRepository;

        public TeamController( ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        [HttpGet]
        [Route("getTeam/{id}")]
        public async Task<IActionResult> GetTeam([FromRoute] Guid id)
        {
            var team = await this.teamRepository.GetTeam(id);

            if(team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        //[HttpPut]
        //public async Task<IActionResult> update()
        //{
        //}

        [HttpPost("create")]
        public async Task<IActionResult> Create( [FromBody] TeamDomain team)
        {
            await this.teamRepository.Add(team);

            return Ok(team);

        }

    }
}
