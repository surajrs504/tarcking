using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Model.Domain;
using TaskManagement.Model.DTOs;
using TaskManagement.Repositiories;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenInterface tokenRepository;
        private readonly UserManager<ApplicationUserDomain> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthController( UserManager<ApplicationUserDomain> userManager, 
            ITokenInterface tokenRepository, RoleManager<IdentityRole> roleManager)
        {
            this.tokenRepository = tokenRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDTO loginRequest)
        {
            //check if the user exist and then proceed to match the pwd
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            if (user != null)
            {
                var checkPwdResult = await userManager.CheckPasswordAsync(user, loginRequest.Password);
              
                if (checkPwdResult)
                {
                   
                    //get roles for this user 
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //create token
                        var token = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var loginResponseDto = new LoginResponseDto
                        {
                            JWTToken = token
                        };
                        return Ok(loginResponseDto);

                    }

                }
            }
            return BadRequest("Username or password was incorrect");
        }

        //new register method
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDTO regUser)
        {
            var user = new ApplicationUserDomain
            {
                UserName = regUser.Username,
                Email = regUser.Email,
                PhoneNumber=regUser.PhoneNumber,
                IsApproved=false,
                IsRejected=false,
                IsTrackingEnabled=false,
            };

            var result = await userManager.CreateAsync(user, regUser.Password);

            if (!await this.roleManager.RoleExistsAsync("Admin"))
                await this.roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await this.roleManager.RoleExistsAsync("User"))
                await this.roleManager.CreateAsync(new IdentityRole("User"));

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, regUser.Role);
                return Ok(new { message = "User registered successfully." });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userManager.Users.Select(
                u => new UserListResponseDTO
                {
                    UserName = u.UserName?? "" ,
                    Email = u.Email?? "",
                    PhoneNumber = u.PhoneNumber?? ""
                }).ToListAsync();


            return Ok(users);
        }

        [HttpGet("getUserApprovalList")]
        public async Task<IActionResult> GetUserApprovalList()
        {
            var users = await userManager.Users.Where(user => user.IsApproved == false && user.IsRejected == false).ToListAsync();

            return Ok(users);
        }

        [HttpGet("getUserRejectedList")]
        public async Task<IActionResult> GetUserRejectedList()
        {
            var users = await userManager.Users.Where(user => user.IsRejected == true && user.IsApproved == false).ToListAsync();

            return Ok(users);
        }

        [HttpGet("getUserApprovedList")]
        public async Task<IActionResult> GetUserApprovedList()
        {
            var users = await userManager.Users.Where(user => user.IsRejected == false && user.IsApproved == true).ToListAsync();

            return Ok(users);
        }


    }
}
