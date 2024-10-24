using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;

namespace HrManagementSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IEmployeeService _employeeService;
        private readonly IApplicationUserService _userService;
        private readonly ILeaveService _leaveService;
        private readonly IExpenseService _expenseService;
        private readonly INotificationService _notificationService;
        private readonly IEventService _eventService;
        private readonly IDepartmentService _departmentService;
        private readonly ICompanyService _companyService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
             IDashboardService dashboardService,
             IEmployeeService employeeService,
             IApplicationUserService userService,
             ILeaveService leaveService,
             IExpenseService expenseService,
             INotificationService notificationService,
             IEventService eventService,
             IDepartmentService departmentService,
             ICompanyService companyService,
             ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _employeeService = employeeService;
            _userService = userService;
            _leaveService = leaveService;
            _expenseService = expenseService;
            _notificationService = notificationService;
            _eventService = eventService;
            _departmentService = departmentService;
            _companyService = companyService;
            _logger = logger;
        }
        [Authorize(Roles = "Employee,Manager,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
               var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    _logger.LogWarning("User email not found in claims");
                    return BadRequest("User email not found in the token");
                }

                _logger.LogInformation($"Attempting to get dashboard for user email: {userEmail}");

                var user = await _userService.GetUserByEmailAsync(userEmail);
                if (user == null)
                {
                    _logger.LogWarning($"User not found in the database for email: {userEmail}");
                    return NotFound($"User not found in the database for email: {userEmail}");
                }

                var userRole = user.Role;
                if (string.IsNullOrEmpty(userRole))
                {
                    _logger.LogWarning($"Role not found for user with email: {userEmail}");
                    return BadRequest($"Role not found for user with email: {userEmail}");
                }

                _logger.LogInformation($"User role: {userRole}");

                switch (userRole)
                {
                    case "Employee":
                        var employeeDashboard = await _dashboardService.GetEmployeeDashboardAsync(userEmail);
                        return Ok(employeeDashboard);

                    case "Manager":
                        var managerDashboard = await _dashboardService.GetManagerDashboardAsync(userEmail);
                        return Ok(managerDashboard);

                    case "Admin":
                        var adminDashboard = await _dashboardService.GetAdminDashboardAsync();
                        return Ok(adminDashboard);

                    default:
                        _logger.LogWarning($"Invalid user role: {userRole}");
                        return BadRequest($"Invalid user role: {userRole}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the dashboard");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }

        [HttpGet("employee-summary")]
        public async Task<IActionResult> GetEmployeeSummary()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var employee = await _employeeService.GetEmployeeByIdAsync(userId);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            var leaves = await _leaveService.GetLeavesByEmployeeAsync((employee.EmployeeId));
            var expenses = await _expenseService.GetExpensesByEmployeeAsync(employee.EmployeeId);

            var summary = new EmployeeSummaryDto
            {
                Employee = employee,
                TotalLeavesTaken = leaves.Count(l => l.Status == "Approved"),
                TotalExpensesSubmitted = expenses.Sum(e => e.Amount)
            };

            return Ok(summary);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet("company-overview")]
        public async Task<IActionResult> GetCompanyOverview()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var company = await _companyService.GetCompanyByIdAsync(user.CompanyId);
            var departments = await _departmentService.GetDepartmentsByCompanyAsync(user.CompanyId);
            var employees = await _employeeService.GetEmployeesByCompanyAsync(user.CompanyId);

            // Get all leaves and expenses for the company
            var allLeaves = await _leaveService.GetPaginatedLeavesAsync(user.CompanyId, 1, int.MaxValue);
            var allExpenses = await _expenseService.GetPaginatedExpensesAsync(user.CompanyId, 1, int.MaxValue);

            var departmentSummaries = departments.Select(d => new DepartmentSummaryDto
            {
                DepartmentId = d.Id,
                DepartmentName = d.Name,
                EmployeeCount = employees.Count(e => e.DepartmentId == d.Id),
                PendingLeaveRequests = allLeaves.Items.Count(l => l.Status == "Pending" && employees.Any(e => e.EmployeeId == l.EmployeeId && e.DepartmentId == d.Id)),
                TotalExpensesThisMonth = allExpenses.Items
                    .Where(e => e.ExpenseDate.Month == DateTime.Now.Month && e.ExpenseDate.Year == DateTime.Now.Year && employees.Any(emp => emp.EmployeeId == e.EmployeeId && emp.DepartmentId == d.Id))
                    .Sum(e => e.Amount)
            }).ToList();

            var companyOverview = new CompanyOverviewDto
            {
                Company = company,
                TotalDepartments = departments.Count(),
                Departments = departmentSummaries
            };

            return Ok(companyOverview);
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetRecentNotifications()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var notifications = await _notificationService.GetNotificationsByEmployeeAsync(userId);
            return Ok(notifications.OrderByDescending(n => n.DateSent).Take(5));
        }

        [HttpGet("upcoming-events")]
        public async Task<IActionResult> GetUpcomingEvents()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var events = await _eventService.GetUpcomingEventsAsync(user.CompanyId, 30); // Get events for next 30 days
            return Ok(events);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet("pending-approvals")]
        public async Task<IActionResult> GetPendingApprovals()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var pendingLeaves = await _leaveService.GetPaginatedLeavesAsync(user.CompanyId, 1, 1000);
            var pendingExpenses = await _expenseService.GetPaginatedExpensesAsync(user.CompanyId, 1, 1000);

            var pendingApprovals = new PendingApprovalsDto
            {
                PendingLeaves = pendingLeaves.Items.Where(l => l.Status == "Pending").ToList(),
                PendingExpenses = pendingExpenses.Items.Where(e => e.Status == "Pending").ToList()
            };

            return Ok(pendingApprovals);
        }
    }

   
}