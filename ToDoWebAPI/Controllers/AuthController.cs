using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoWebAPI.Dtos;
using ToDoWebAPI.Service.Interface;

namespace ToDoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _userService;

        public AuthController(IUser userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            var result = await _userService.CreateUserAsync(createUserDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDtos.RefreshTokenRequestDto refreshTokenDto)
        {
            var result = await _userService.RefreshToken(refreshTokenDto);
            return Ok(result);
        }
    }
}
