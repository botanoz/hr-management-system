using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.UnitOfWork;
using HrManagementSystem.DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace HrManagementSystem.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation($"Attempting login for user with email: {loginDto.Email}");

                var user = await _unitOfWork.ApplicationUsers.GetUserByEmailAsync(loginDto.Email);

                if (user == null)
                {
                    _logger.LogWarning($"User with email {loginDto.Email} not found");
                    return new AuthResultDto { Succeeded = false, Error = "Invalid email or password." };
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!passwordCheck)
                {
                    _logger.LogWarning($"Invalid password for user with email {loginDto.Email}");
                    return new AuthResultDto { Succeeded = false, Error = "Invalid email or password." };
                }

                _logger.LogInformation($"User {loginDto.Email} logged in successfully");
                return await GenerateAuthResultForUserAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during login for user {loginDto.Email}");
                return new AuthResultDto { Succeeded = false, Error = "An error occurred during login. Please try again." };
            }
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation($"Attempting to register new user with email: {registerDto.Email}");

                var existingUser = await _unitOfWork.ApplicationUsers.GetUserByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning($"User with email {registerDto.Email} already exists");
                    return new AuthResultDto { Succeeded = false, Error = "User with this email already exists." };
                }

                var user = _mapper.Map<ApplicationUser>(registerDto);
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning($"Failed to create user {registerDto.Email}. Errors: {errors}");
                    return new AuthResultDto { Succeeded = false, Error = errors };
                }

                await _userManager.AddToRoleAsync(user, "Employee");

                _logger.LogInformation($"User {registerDto.Email} registered successfully");
                return await GenerateAuthResultForUserAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during registration for user {registerDto.Email}");
                return new AuthResultDto { Succeeded = false, Error = "An error occurred during registration. Please try again." };
            }
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _unitOfWork.ApplicationUsers.GetUserWithDetailsAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning($"User with id {userId} not found");
                    return null;
                }

                var userDto = _mapper.Map<UserDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Role = roles.FirstOrDefault();

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user with id {userId}");
                return null;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                var user = await _unitOfWork.ApplicationUsers.GetByIdAsync(int.Parse(userId));
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating token");
                return false;
            }
        }

        public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var users = await _unitOfWork.ApplicationUsers.GetAllAsync();
                var user = users.FirstOrDefault(u => u.RefreshToken == refreshToken);

                if (user == null)
                {
                    _logger.LogWarning("Invalid refresh token");
                    return new AuthResultDto { Succeeded = false, Error = "Invalid refresh token." };
                }

                if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    _logger.LogWarning("Refresh token expired");
                    return new AuthResultDto { Succeeded = false, Error = "Refresh token expired." };
                }

                return await GenerateAuthResultForUserAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing token");
                return new AuthResultDto { Succeeded = false, Error = "An error occurred while refreshing token. Please try again." };
            }
        }

        private async Task<AuthResultDto> GenerateAuthResultForUserAsync(ApplicationUser user)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogError("Attempt to generate token for null user");
                    throw new ArgumentNullException(nameof(user));
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT:Secret is not configured"));

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("FirstName", user.FirstName ?? string.Empty),
            new Claim("LastName", user.LastName ?? string.Empty),
            new Claim("Position", user.Position ?? string.Empty),
            new Claim("CompanyId", user.CompanyId.ToString()),
            new Claim("IsManager", user.IsManager.ToString())
        };

                var userRoles = await _userManager.GetRolesAsync(user);
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                TimeSpan tokenLifetime;
                if (!TimeSpan.TryParse(_configuration["JWT:TokenLifetime"], out tokenLifetime))
                {
                    _logger.LogWarning("Invalid JWT:TokenLifetime configuration. Using default value of 1 hour.");
                    tokenLifetime = TimeSpan.FromHours(1);
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.Add(tokenLifetime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["JWT:ValidIssuer"],
                    Audience = _configuration["JWT:ValidAudience"]
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    _logger.LogError($"Failed to update user with refresh token: {string.Join(", ", updateResult.Errors)}");
                    throw new InvalidOperationException("Failed to update user with refresh token");
                }

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Role = userRoles.FirstOrDefault();

                _logger.LogInformation($"Generated auth result for user: {user.Email}");

                return new AuthResultDto
                {
                    Succeeded = true,
                    AccessToken = tokenHandler.WriteToken(token),
                    RefreshToken = refreshToken,
                    ExpiresAt = tokenDescriptor.Expires.Value,
                    User = userDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating auth result");
                throw;
            }
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}