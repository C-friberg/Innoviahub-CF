using api.Dtos.UserDtos;
using api.Interfaces;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public AuthController(
            IUserRepository userRepository,
            TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = new User
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.Email
            };

            var created = await _userRepository.CreateUserAsync(
                user,
                registerUserDto.Password
            );

            if (!created)
            {
                return BadRequest("Kunde inte skapa användare");
            }

            return Ok("Användaren skapades");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Fel email eller lösenord.");
            }

            var passwordCorrect = await _userRepository.CheckPasswordAsync(
                user,
                loginDto.Password
            );

            if (!passwordCorrect)
            {
                return Unauthorized("Fel email eller lösenord.");
            }

            var token = await _tokenService.CreateToken(user);

            return Ok(new { token });
        }
    }
}