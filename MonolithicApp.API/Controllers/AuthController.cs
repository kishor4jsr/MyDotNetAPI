using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithicApp.Models;
using MonolithicApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonolithicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            bool existingUser =await  _authService.FindByNameAsync(request.Username);
            if (existingUser)
            {
                return Conflict("User already exists.");
            }

            var result = await _authService.RegisterAsync(request);
            if (!result) return BadRequest("Registration failed.");

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and Password are required.");
            }


            var token = await _authService.LoginAsync(request.Username, request.Password);
            if (token == null) return Unauthorized();

            return Ok(token);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var newToken = await _authService.RefreshTokenAsync(refreshToken);
            if (newToken == null) return Unauthorized();

            return Ok(newToken);
        }

    }
}
