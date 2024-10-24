using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;

namespace HrManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IApplicationUserService _userService;
        private readonly IEmployeeService _employeeService;

        public AdminController(
            ICompanyService companyService,
            IApplicationUserService userService,
            IEmployeeService employeeService)
        {
            _companyService = companyService;
            _userService = userService;
            _employeeService = employeeService;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<AdminDashboardDto>> GetDashboard()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            var users = await GetAllUsersAsync();

            var dashboard = new AdminDashboardDto
            {
                TotalCompanies = companies.Count(),
                PendingCompanyApprovals = companies.Count(c => !c.IsApproved),
                TotalUsers = users.Count(),
                ActiveSubscriptions = companies.Count(c => c.SubscriptionEndDate > DateTime.Now),
                RecentlyRegisteredCompanies = companies
                    .OrderByDescending(c => c.RegistrationDate)
                    .Take(5)
                    .Select(c => new CompanySummaryDto
                    {
                        CompanyId = c.Id,
                        CompanyName = c.Name,
                        RegistrationDate = c.RegistrationDate,
                        EmployeeCount = c.EmployeeCount
                    })
                    .ToList(),
                UpcomingSubscriptionExpirations = companies
                    .Where(c => c.SubscriptionEndDate <= DateTime.Now.AddDays(30))
                    .OrderBy(c => c.SubscriptionEndDate)
                    .Take(5)
                    .Select(c => new CompanySummaryDto
                    {
                        CompanyId = c.Id,
                        CompanyName = c.Name,
                        SubscriptionEndDate = c.SubscriptionEndDate,
                        EmployeeCount = c.EmployeeCount
                    })
                    .ToList()
            };

            return Ok(dashboard);
        }

        [HttpPost("approve-company")]
        public async Task<IActionResult> ApproveCompany(CompanyApprovalDto approvalDto)
        {
            if (approvalDto.IsApproved)
            {
                await _companyService.ApproveCompanyAsync(approvalDto.CompanyId);
            }
            else
            {
                await _companyService.RejectCompanyAsync(approvalDto.CompanyId, approvalDto.RejectionReason);
            }
            return Ok();
        }

        [HttpPut("update-subscription")]
        public async Task<IActionResult> UpdateSubscription(SubscriptionUpdateDto updateDto)
        {
            await _companyService.UpdateCompanySubscriptionAsync(updateDto.CompanyId, updateDto.NewEndDate);
            return Ok();
        }

        [HttpGet("companies")]
        public async Task<ActionResult<PaginatedResultDto<CompanyDto>>> GetCompanies(int pageNumber = 1, int pageSize = 10)
        {
            var paginatedResult = await _companyService.GetPaginatedCompaniesAsync(pageNumber, pageSize);
            return Ok(paginatedResult);
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut("update-user-role")]
        public async Task<IActionResult> UpdateUserRole(UserRoleUpdateDto updateDto)
        {
            await _userService.SetUserRoleAsync(Guid.Parse(updateDto.UserId), updateDto.NewRole);
            return Ok();
        }

        [HttpGet("company/{companyId}/employees")]
        public async Task<ActionResult<PaginatedResultDto<EmployeeDto>>> GetCompanyEmployees(int companyId, int pageNumber = 1, int pageSize = 10)
        {
            var paginatedResult = await _employeeService.GetPaginatedEmployeesAsync(companyId, pageNumber, pageSize);
            return Ok(paginatedResult);
        }

        [HttpDelete("company/{companyId}")]
        public async Task<IActionResult> DeleteCompany(int companyId)
        {
            await _companyService.DeleteCompanyAsync(companyId);
            return Ok();
        }

        [HttpPut("deactivate-user/{userId}")]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            var result = await _userService.DeactivateUserAsync(Guid.Parse(userId));
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("activate-user/{userId}")]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            var result = await _userService.ActivateUserAsync(Guid.Parse(userId));
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }

        private async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            var allUsers = new List<UserDto>();

            foreach (var company in companies)
            {
                var companyUsers = await _userService.GetUsersByCompanyAsync(company.Id);
                allUsers.AddRange(companyUsers);
            }

            return allUsers;
        }
    }

    public class CompanyApprovalDto
    {
        public int CompanyId { get; set; }
        public bool IsApproved { get; set; }
        public string RejectionReason { get; set; }
    }

    public class SubscriptionUpdateDto
    {
        public int CompanyId { get; set; }
        public DateTime NewEndDate { get; set; }
    }

    public class UserRoleUpdateDto
    {
        public string UserId { get; set; }
        public string NewRole { get; set; }
    }
}