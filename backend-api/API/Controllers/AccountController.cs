using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.Entities;
using System.Linq;

namespace HrManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IApplicationUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            IAuthService authService,
            IApplicationUserService userService,
            ICompanyService companyService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registerDto);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully", User = result.User });
            }

            return BadRequest(new { Message = "User registration failed", Errors = result.Error });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    Token = result.AccessToken,
                    ExpiresAt = result.ExpiresAt,
                    User = result.User
                });
            }

            return Unauthorized(new { Message = "Invalid login attempt", Error = result.Error });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userDto = await _userService.GetUserByIdAsync(userId);
            if (userDto == null)
            {
                return NotFound("User not found");
            }

            return Ok(userDto);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto userDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != userDto.Id)
            {
                return Forbid();
            }

            var result = await _userService.UpdateUserAsync(userDto);
            if (result)
            {
                return Ok(new { Message = "Profile updated successfully" });
            }

            return BadRequest("Failed to update profile");
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _userService.ChangePasswordAsync(userId, changePasswordDto);
            if (result)
            {
                return Ok(new { Message = "Password changed successfully" });
            }

            return BadRequest("Failed to change password");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-company")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyDto companyDto)
        {
            var result = await _companyService.CreateCompanyAsync(companyDto);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Failed to create company");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign-company")]
        public async Task<IActionResult> AssignCompany([FromBody] AssignCompanyDto assignCompanyDto)
        {
            var user = await _userService.GetUserByIdAsync(Guid.Parse(assignCompanyDto.UserId));
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.CompanyId = assignCompanyDto.CompanyId;
            var result = await _userService.UpdateUserAsync(user);
            if (result)
            {
                return Ok(new { Message = "Company assigned successfully" });
            }

            return BadRequest("Failed to assign company");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    Token = result.AccessToken,
                    RefreshToken = result.RefreshToken,
                    ExpiresAt = result.ExpiresAt
                });
            }

            return BadRequest(new { Message = "Invalid refresh token", Error = result.Error });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "User logged out successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(Guid.Parse(id));
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(Guid.Parse(id));
            if (result)
            {
                return Ok(new { Message = "User deleted successfully" });
            }
            return BadRequest("Failed to delete user");
        }
    }

    public class AssignCompanyDto
    {
        public string UserId { get; set; }
        public int CompanyId { get; set; }
    }

    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }
    }
}