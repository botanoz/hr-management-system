using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace HrManagementSystem.BusinessLogic.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DashboardService> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveService _leaveService;
        private readonly IExpenseService _expenseService;
        private readonly IShiftService _shiftService;
        private readonly ICompanyService _companyService;

        public DashboardService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DashboardService> logger,
            IEmployeeService employeeService,
            ILeaveService leaveService,
            IExpenseService expenseService,
            IShiftService shiftService,
            ICompanyService companyService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _employeeService = employeeService;
            _leaveService = leaveService;
            _expenseService = expenseService;
            _shiftService = shiftService;
            _companyService = companyService;
        }

        public async Task<EmployeeDashboardDto> GetEmployeeDashboardAsync(string email)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByEmailAsync(email);
                if (employee == null)
                {
                    throw new Exception($"Employee with email {email} not found.");
                }

                var leaves = await _leaveService.GetLeavesByEmployeeAsync(employee.EmployeeId);
                var expenses = await _expenseService.GetExpensesByEmployeeAsync(employee.EmployeeId);
                var shifts = await _shiftService.GetShiftsByEmployeeAsync(employee.EmployeeId);

                var dashboard = new EmployeeDashboardDto
                {
                    PendingLeaveRequests = leaves.Count(l => l.Status == "Pending"),
                    PendingExpenseRequests = expenses.Count(e => e.Status == "Pending"),
                    UpcomingShifts = shifts.Where(s => s.StartTime > DateTime.Now).OrderBy(s => s.StartTime).Take(5).ToList(),
                    RemainingLeaveDays = await _leaveService.GetRemainingLeaveDaysAsync(employee.EmployeeId),
                    TotalExpensesThisMonth = expenses
                        .Where(e => e.ExpenseDate.Month == DateTime.Now.Month && e.ExpenseDate.Year == DateTime.Now.Year)
                        .Sum(e => e.Amount),
                    RecentNotifications = await GetRecentNotificationsAsync(employee.CompanyId, employee.EmployeeId),
                    UpcomingEvents = await GetUpcomingEventsAsync(employee.CompanyId),
                    UpcomingHolidays = await GetUpcomingHolidaysAsync(employee.CompanyId)
                };

                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while generating employee dashboard for employee {email}");
                throw;
            }
        }

        public async Task<ManagerDashboardDto> GetManagerDashboardAsync(string email)
        {
            try
            {
                var manager = await _employeeService.GetEmployeeByEmailAsync(email);
                if (manager == null)
                {
                    throw new Exception($"Manager with email {email} not found.");
                }

                var employees = await _employeeService.GetEmployeesByCompanyAsync(manager.CompanyId);
                var leaves = await GetAllLeavesForCompanyAsync(manager.CompanyId);
                var expenses = await GetAllExpensesForCompanyAsync(manager.CompanyId);

                var dashboard = new ManagerDashboardDto
                {
                    TotalEmployees = employees.Count(),
                    PendingLeaveRequests = leaves.Count(l => l.Status == "Pending"),
                    PendingExpenseRequests = expenses.Count(e => e.Status == "Pending"),
                    TotalExpensesThisMonth = expenses
                        .Where(e => e.ExpenseDate.Month == DateTime.Now.Month && e.ExpenseDate.Year == DateTime.Now.Year)
                        .Sum(e => e.Amount),
                    UpcomingBirthdays = await GetUpcomingBirthdaysAsync(manager.CompanyId),
                    ActiveShifts = await GetActiveShiftsCountAsync(manager.CompanyId),
                    DepartmentSummaries = await GetDepartmentSummariesAsync(manager.CompanyId),
                    RecentNotifications = await GetRecentNotificationsAsync(manager.CompanyId),
                    UpcomingEvents = await GetUpcomingEventsAsync(manager.CompanyId),
                    UpcomingHolidays = await GetUpcomingHolidaysAsync(manager.CompanyId)
                };

                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while generating manager dashboard for manager {email}");
                throw;
            }
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync();
                var users = await _unitOfWork.ApplicationUsers.GetAllAsync();

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
                        .ToList(),
                    RecentNotifications = await GetRecentNotificationsAsync(null),
                    UpcomingEvents = await GetUpcomingEventsAsync(null),
                    UpcomingHolidays = await GetUpcomingHolidaysAsync(null)
                };

                return dashboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating admin dashboard");
                throw;
            }
        }

        private async Task<List<EmployeeDto>> GetUpcomingBirthdaysAsync(int companyId)
        {
            var nextMonth = DateTime.Now.AddMonths(1);
            var employees = await _employeeService.GetEmployeesByCompanyAsync(companyId);
            return employees
                .Where(e => e.Birthdate.Month == DateTime.Now.Month || e.Birthdate.Month == nextMonth.Month)
                .OrderBy(e => e.Birthdate.Month)
                .ThenBy(e => e.Birthdate.Day)
                .Take(5)
                .ToList();
        }

        private async Task<int> GetActiveShiftsCountAsync(int companyId)
        {
            var now = DateTime.Now;
            var employees = await _employeeService.GetEmployeesByCompanyAsync(companyId);
            var activeShifts = 0;

            foreach (var employee in employees)
            {
                var shifts = await _shiftService.GetShiftsByEmployeeAsync(employee.EmployeeId);
                activeShifts += shifts.Count(s => s.StartTime <= now && s.EndTime >= now);
            }

            return activeShifts;
        }

        private async Task<List<DepartmentSummaryDto>> GetDepartmentSummariesAsync(int companyId)
        {
            var departments = await _unitOfWork.Departments.GetDepartmentsByCompanyAsync(companyId);
            var summaries = new List<DepartmentSummaryDto>();

            foreach (var department in departments)
            {
                var employees = await _employeeService.GetEmployeesByCompanyAsync(companyId);
                var departmentEmployees = employees.Where(e => e.DepartmentId == department.Id).ToList();

                var summary = new DepartmentSummaryDto
                {
                    DepartmentId = department.Id,
                    DepartmentName = department.Name,
                    EmployeeCount = departmentEmployees.Count,
                    PendingLeaveRequests = (await GetAllLeavesForCompanyAsync(companyId))
                        .Count(l => departmentEmployees.Any(e => e.EmployeeId == l.EmployeeId) && l.Status == "Pending"),
                    TotalExpensesThisMonth = (await GetAllExpensesForCompanyAsync(companyId))
                        .Where(e => departmentEmployees.Any(emp => emp.EmployeeId == e.EmployeeId) &&
                                    e.ExpenseDate.Month == DateTime.Now.Month &&
                                    e.ExpenseDate.Year == DateTime.Now.Year)
                        .Sum(e => e.Amount)
                };

                summaries.Add(summary);
            }

            return summaries;
        }

        private async Task<List<NotificationDto>> GetRecentNotificationsAsync(int? companyId, Guid? employeeId = null)
        {
            var allNotifications = await _unitOfWork.Notifications.GetAllAsync();

            IEnumerable<NotificationDto> filteredNotifications;

            if (employeeId.HasValue)
            {
                filteredNotifications = allNotifications
                    .Where(n => n.EmployeeId == employeeId)
                    .OrderByDescending(n => n.DateSent)
                    .Take(5)
                    .Select(_mapper.Map<NotificationDto>);
            }
            else if (companyId.HasValue)
            {
                var employees = await _employeeService.GetEmployeesByCompanyAsync(companyId.Value);
                var employeeIds = employees.Select(e => e.EmployeeId).ToList();

                filteredNotifications = allNotifications
                    .Where(n => (n.EmployeeId.HasValue && employeeIds.Contains(n.EmployeeId.Value)) || n.CompanyId == companyId)
                    .OrderByDescending(n => n.DateSent)
                    .Take(5)
                    .Select(_mapper.Map<NotificationDto>);
            }
            else
            {
                filteredNotifications = allNotifications
                    .OrderByDescending(n => n.DateSent)
                    .Take(5)
                    .Select(_mapper.Map<NotificationDto>);
            }

            return filteredNotifications.ToList();
        }

        private async Task<List<EventDto>> GetUpcomingEventsAsync(int? companyId)
        {
            var now = DateTime.Now;
            var thirtyDaysLater = now.AddDays(30);

            var allEvents = await _unitOfWork.Events.GetAllAsync();

            IEnumerable<EventDto> filteredEvents;

            if (companyId.HasValue)
            {
                filteredEvents = allEvents
                    .Where(e => e.CompanyId == companyId && e.EventDate >= now && e.EventDate <= thirtyDaysLater)
                    .OrderBy(e => e.EventDate)
                    .Take(5)
                    .Select(_mapper.Map<EventDto>);
            }
            else
            {
                filteredEvents = allEvents
                    .Where(e => e.EventDate >= now && e.EventDate <= thirtyDaysLater)
                    .OrderBy(e => e.EventDate)
                    .Take(5)
                    .Select(_mapper.Map<EventDto>);
            }

            return filteredEvents.ToList();
        }

        private async Task<List<HolidayDto>> GetUpcomingHolidaysAsync(int? companyId)
        {
            var now = DateTime.Now;
            var thirtyDaysLater = now.AddDays(30);

            var allHolidays = await _unitOfWork.Holidays.GetAllAsync();

            IEnumerable<HolidayDto> filteredHolidays;

            if (companyId.HasValue)
            {
                filteredHolidays = allHolidays
                    .Where(h => h.CompanyId == companyId && h.Date >= now && h.Date <= thirtyDaysLater)
                    .OrderBy(h => h.Date)
                    .Take(5)
                    .Select(_mapper.Map<HolidayDto>);
            }
            else
            {
                filteredHolidays = allHolidays
                    .Where(h => h.Date >= now && h.Date <= thirtyDaysLater)
                    .OrderBy(h => h.Date)
                    .Take(5)
                    .Select(_mapper.Map<HolidayDto>);
            }

            return filteredHolidays.ToList();
        }

        private async Task<List<LeaveDto>> GetAllLeavesForCompanyAsync(int companyId)
        {
            var employees = await _employeeService.GetEmployeesByCompanyAsync(companyId);
            var allLeaves = new List<LeaveDto>();
            foreach (var employee in employees)
            {
                var employeeLeaves = await _leaveService.GetLeavesByEmployeeAsync(employee.EmployeeId);
                allLeaves.AddRange(employeeLeaves);
            }
            return allLeaves;
        }

        private async Task<List<ExpenseDto>> GetAllExpensesForCompanyAsync(int companyId)
        {
            var employees = await _employeeService.GetEmployeesByCompanyAsync(companyId);
            var allExpenses = new List<ExpenseDto>();
            foreach (var employee in employees)
            {
                var employeeExpenses = await _expenseService.GetExpensesByEmployeeAsync(employee.EmployeeId);
                allExpenses.AddRange(employeeExpenses);
            }
            return allExpenses;
        }
    }
}