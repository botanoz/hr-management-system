using AutoMapper;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationUserService> _logger;

        public ApplicationUserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<ApplicationUserService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            try
            {
                _logger.LogInformation($"Attempting to get user with ID: {userId}");

                var user = await _unitOfWork.ApplicationUsers.GetUserWithDetailsAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning($"User not found in database with ID: {userId}");
                    return null;
                }

                _logger.LogInformation($"User found: {user.Email}");

                var userDto = _mapper.Map<UserDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Role = roles.FirstOrDefault();

                _logger.LogInformation($"User role: {userDto.Role}");

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user with ID: {userId}");
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersByCompanyAsync(int companyId)
        {
            try
            {
                var users = await _unitOfWork.ApplicationUsers.GetUsersByCompanyAsync(companyId);
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting users for company ID: {companyId}");
                throw;
            }
        }

        public async Task<UserDto> CreateUserAsync(RegisterDto registerDto)
        {
            try
            {
                var user = _mapper.Map<ApplicationUser>(registerDto);
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    await _unitOfWork.CompleteAsync();
                    return _mapper.Map<UserDto>(user);
                }
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating user: {registerDto.Email}");
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            try
            {
                var user = await _unitOfWork.ApplicationUsers.GetByIdAsync(Guid.Parse(userDto.Id));
                if (user == null) return false;
                _mapper.Map(userDto, user);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user: {userDto.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return false;
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    await _unitOfWork.CompleteAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user: {userId}");
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return false;
                var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while changing password for user: {userId}");
                throw;
            }
        }

        public async Task<string> GetUserRoleAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return null;
                var roles = await _userManager.GetRolesAsync(user);
                return roles.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting role for user: {userId}");
                throw;
            }
        }

        public async Task<bool> SetUserRoleAsync(Guid userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return false;
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while setting role for user: {userId}");
                throw;
            }
        }

        public async Task<bool> DeactivateUserAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return false;
                user.IsManager = false;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _unitOfWork.CompleteAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deactivating user: {userId}");
                throw;
            }
        }

        public async Task<bool> ActivateUserAsync(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return false;
                user.IsManager = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _unitOfWork.CompleteAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while activating user: {userId}");
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.ApplicationUsers.GetAllAsync();
                var userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    var userDto = _mapper.Map<UserDto>(user);
                    var roles = await _userManager.GetRolesAsync(user);
                    userDto.Role = roles.FirstOrDefault();
                    userDtos.Add(userDto);
                }
                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users");
                throw;
            }
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation($"Attempting to get user with email: {email}");

                var user = await _unitOfWork.ApplicationUsers.GetUserByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning($"User not found with email: {email}");
                    return null;
                }

                var userDto = _mapper.Map<UserDto>(user);

                if (user.CompanyId != 0)
                {
                    var company = await _unitOfWork.Companies.GetByIdAsync(user.CompanyId);
                    if (company != null)
                    {
                        userDto.CompanyName = company.Name;
                    }
                }

                var roles = await _userManager.GetRolesAsync(user);
                userDto.Role = roles.FirstOrDefault();

                _logger.LogInformation($"User found: {user.Email}, Role: {userDto.Role}, Company: {userDto.CompanyName}");

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user with email: {email}");
                throw;
            }
        }
    }
}