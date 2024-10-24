using HrManagementSystem.BusinessLogic.DTOs;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<bool> ValidateTokenAsync(string token);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
    }
}