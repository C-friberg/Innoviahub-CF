using api.Dtos.UserDtos;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
           var users = await _userRepository.GetAllAsync();

           var userDtos = new List<UserDto>();

           foreach (var user in users)
           {
               var roles = await _userRepository.GetRolesAsync(user);

               userDtos.Add(new UserDto
               {
                   UserId = user.Id,
                   Email = user.Email ?? string.Empty,
                   Role = roles.Contains("Admin") ? "Admin" : "User"
                });
            }

            return Ok(userDtos);
        }
    }
}

