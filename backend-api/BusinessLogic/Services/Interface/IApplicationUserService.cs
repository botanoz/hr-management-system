using HrManagementSystem.BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IApplicationUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetUsersByCompanyAsync(int companyId);
        Task<UserDto> CreateUserAsync(RegisterDto registerDto);
        Task<bool> UpdateUserAsync(UserDto userDto);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
        Task<string> GetUserRoleAsync(Guid userId);
        Task<bool> SetUserRoleAsync(Guid userId, string roleName);
        Task<bool> DeactivateUserAsync(Guid userId);
        Task<bool> ActivateUserAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByEmailAsync(string email);
    }
}