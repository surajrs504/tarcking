using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Model.DTOs;
using TaskManagement.Repositiories;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenInterface tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenInterface tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDTO authRequest)
        {
            Console.WriteLine(authRequest);
            var identityUser = new IdentityUser
            {
                UserName = authRequest.Username,
                Email = authRequest.Email,
                PhoneNumber=authRequest.PhoneNumber,
                
            };
            Console.WriteLine( identityUser);
            var identityResult = await userManager.CreateAsync(identityUser, authRequest.Password);
            Console.WriteLine( identityResult);
            if (identityResult.Succeeded)
            {       
                        return Ok("User was registered, Please login");
                   
            }
            return BadRequest("Somthing went wrong");
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

    

}
}
