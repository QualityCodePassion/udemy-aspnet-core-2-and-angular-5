using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialApp.API.Data;
using SocialApp.API.Dtos;
using SocialApp.API.Helpers;
using SocialApp.API.Logging;

namespace SocialApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly ISocialAppRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(ISocialAppRepository repo, IMapper mapper, ILogger<UsersController> logger)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(UserParams userParams)
        {
            // Note the the default model binding to fill in the userParams is handled
            // automatically by MVC and if an error orrurs it will be indicated by
            // setting the ModelState.IsValid to false 
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.InvalidModelState,
                        "GetUsers method detected invalid model state");
                return BadRequest(ModelState);
            }

            // Get the name of the currently logged in user
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);
            userParams.UserId = currentUserId;
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                // Since the author of this course was building a "Dating site",
                // he was assuming that by default only other users of the opposite
                // sex would be retrieved
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }
            
            // Read the userParams
            var users = await _repo.GetUsers(userParams);
            var returnUsers = _mapper.Map<IEnumerable<UserForListDto>>(users.Items);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(returnUsers);
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> GetUser(int id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(LoggingEvents.InvalidModelState,
                        "GetUser method detected invalid model state");
                return BadRequest(ModelState);
            }

            var user = await _repo.GetUser(id);
            var returnUser = _mapper.Map<UserForDetailed>(user);

            return Ok(returnUser);
        }
    }
}