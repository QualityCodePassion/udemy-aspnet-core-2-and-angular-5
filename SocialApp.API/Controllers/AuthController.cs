using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SocialApp.API.Data;
using SocialApp.API.Dtos;
using SocialApp.API.Logging;
using SocialApp.API.Models;

namespace SocialApp.API.Controllers
{

    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepo;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository authRepository,
            IConfiguration config,
            ILogger<AuthController> logger)
        {
            _config = config;
            _authRepo = authRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            userDto.Username = userDto.Username.ToLower();
            bool userExists = false;

            if (await _authRepo.UserExists(userDto.Username))
            {
                userExists = true;
                ModelState.AddModelError("Username", "Username already exists");
                _logger.LogInformation(LoggingEvents.UserExists, "Username already exist: {Username}",
                    userDto.Username);
            }

            if (!ModelState.IsValid)
            {
                if (!userExists)
                    _logger.LogWarning(LoggingEvents.InvalidModelState,
                        "Register controller detected invalid model state");
                
                return BadRequest(ModelState);
            }

            var newUser = new User
            {
                Username = userDto.Username
            };

            var createdUser = await _authRepo.Register(newUser, userDto.Password);

            // TODO change this to a "CreatedAtRoute"
            return StatusCode(201);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterUserDto user)
        {
            var loggedInUser = await _authRepo.Login(user.Username.ToLower(),
                 user.Password);

            // If the user isn't logged in successfully, return "Unauthorised",
            // but don't give any hints about why.
            
            // TODO Only allow a certain amount of unsuccessful login attempts
            // for a given user in a given time period before locking out
            // the user until for a set time.
            if (loggedInUser == null)
                return Unauthorized();

            // Generate JSON Web Token (JWT)
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, loggedInUser.Username)
                }),
                // TODO put this into a config and make it smaller
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDesc);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { tokenString });
        }
    }
}