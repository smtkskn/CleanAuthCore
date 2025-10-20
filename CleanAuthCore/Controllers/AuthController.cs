using System.Runtime.CompilerServices;
using CleanAuthCore.Entities;
using CleanAuthCore.Repositories.Interfaces;
using CleanAuthCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanAuthCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var existingUser = await _userRepository.GetUserByUserNameAsync(user.UserName);
            if (existingUser != null) return BadRequest("Kullanıcı zaten mevcut.");

            await _userRepository.AddUserAsync(user);
            return Ok("Kayıt başarılı.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var isValid = await _userRepository.ValidateUserAsync(login.UserName, login.PasswordHash);
            if (!isValid)
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");
            var user = await _userRepository.GetUserByUserNameAsync(login.UserName);
            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }
        [HttpGet("profile")]
        [Authorize] 
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity.Name;
            var role = User.FindFirst("role")?.Value;

            return Ok(new
            {
                UserName = username,
                Role = role,
            });
        }
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Bu endpoint sadece adminlere açıktır!");
        }
     
    }
}
