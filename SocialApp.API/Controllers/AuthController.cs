using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialApp.API.Data;
using SocialApp.API.Dtos;
using SocialApp.API.Models;

namespace SocialApp.API.Controllers
{
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepository)
        {
            this._authRepo = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            userDto.Username = userDto.Username.ToLower();

            if( await _authRepo.UserExists(userDto.Username) )
                ModelState.AddModelError("Username", "Username already exists");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new User
            {
                Username = userDto.Username
            };

            var createdUser = await _authRepo.Register(newUser, userDto.Password);

            // TODO change this to a "CreatedAtRoute"
            return StatusCode(201);
        }
    }
}