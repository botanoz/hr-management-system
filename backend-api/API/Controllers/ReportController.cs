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
    [Authorize(Roles = "Admin,Manager")]
    public class ReportController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveService _leaveService;
        private readonly IExpenseService _expenseService;
        private readonly IShiftService _shiftService;
        private readonly ICompanyService _companyService;

        public ReportController(
            IEmployeeService employeeService,
            ILeaveService leaveService,
            IExpenseService expenseService,
            IShiftService shiftService,
            ICompanyService companyService)
        {
            _employeeService = employeeService;
            _leaveService = leaveService;
            _expenseService = expenseService;
            _shiftService = shiftService;
            _companyService = companyService;
        }

        [HttpGet("employee")]
        public async Task<ActionResult<EmployeeReportDto>> GetEmployeeReport(int companyId)
        {
            var employees = await _employeeService.GetEmployeesByCompanyAsync(companyId);
            var currentDate = DateTime.Now;

            var report = new EmployeeReportDto
            {
                TotalEmployees = employees.Count(),
                EmployeesByDepartment = employees.GroupBy(e => e.DepartmentName)
                    .ToDictionary(g => g.Key, g => g.Count()),
                EmployeesByPosition = employees.GroupBy(e => e.Position)
                    .ToDictionary(g => g.Key, g => g.Count()),
                EmployeesByYearsOfService = employees.GroupBy(e => (int)(currentDate - e.HireDate).TotalDays / 365)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return Ok(report);
        }

        [HttpGet("leave")]
        public async Task<ActionResult<LeaveReportDto>> GetLeaveReport(int companyId, DateTime startDate, DateTime endDate)
        {
            var leaves = await _leaveService.GetPaginatedLeavesAsync(companyId, 1, int.MaxValue);
            var filteredLeaves = leaves.Items.Where(l => l.StartDate >= startDate && l.EndDate <= endDate);

            var report = new LeaveReportDto
            {
                TotalLeaveRequests = filteredLeaves.Count(),
                ApprovedLeaves = filteredLeaves.Count(l => l.Status == "Approved"),
                PendingLeaves = filteredLeaves.Count(l => l.Status == "Pending"),
                RejectedLeaves = filteredLeaves.Count(l => l.Status == "Rejected"),
                LeavesByType = filteredLeaves.GroupBy(l => l.LeaveType)
                    .ToDictionary(g => g.Key, g => g.Count()),
                LeavesByDepartment = filteredLeaves.GroupBy(l => l.EmployeeName.Split(' ')[0]) // Assuming department name is the first part of employee name
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return Ok(report);
        }

        [HttpGet("expense")]
        public async Task<ActionResult<ExpenseReportDto>> GetExpenseReport(int companyId, DateTime startDate, DateTime endDate)
        {
            var expenses = await _expenseService.GetPaginatedExpensesAsync(companyId, 1, int.MaxValue);
            var filteredExpenses = expenses.Items.Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate);

            var report = new ExpenseReportDto
            {
                TotalExpenses = filteredExpenses.Sum(e => e.Amount),
                ApprovedExpenses = filteredExpenses.Where(e => e.Status == "Approved").Sum(e => e.Amount),
                PendingExpenses = filteredExpenses.Where(e => e.Status == "Pending").Sum(e => e.Amount),
                RejectedExpenses = filteredExpenses.Where(e => e.Status == "Rejected").Sum(e => e.Amount),
                ExpensesByType = filteredExpenses.GroupBy(e => e.ExpenseType)
                    .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount)),
                ExpensesByDepartment = filteredExpenses.GroupBy(e => e.EmployeeName.Split(' ')[0]) // Assuming department name is the first part of employee name
                    .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount))
            };

            return Ok(report);
        }

        [HttpGet("shift")]
        public async Task<ActionResult<ShiftReportDto>> GetShiftReport(int companyId, DateTime startDate, DateTime endDate)
        {
            var shifts = await _shiftService.GetPaginatedShiftsAsync(companyId, 1, int.MaxValue);
            var filteredShifts = shifts.Items.Where(s => s.StartTime >= startDate && s.EndTime <= endDate);

            var report = new ShiftReportDto
            {
                TotalShifts = filteredShifts.Count(),
                ShiftsByType = filteredShifts.GroupBy(s => s.ShiftType)
                    .ToDictionary(g => g.Key, g => g.Count()),
                ShiftsByDepartment = filteredShifts.GroupBy(s => s.EmployeeName.Split(' ')[0]) // Assuming department name is the first part of employee name
                    .ToDictionary(g => g.Key, g => g.Count()),
                OverTimeHours = filteredShifts.Sum(s => Math.Max(0, (s.EndTime - s.StartTime).Hours - 8)) // Assuming 8 hours is a regular shift
            };

            return Ok(report);
        }

        [HttpGet("company")]
        public async Task<ActionResult<CompanyReportDto>> GetCompanyReport(int companyId, DateTime startDate, DateTime endDate)
        {
            var employeeReport = await GetEmployeeReport(companyId);
            var leaveReport = await GetLeaveReport(companyId, startDate, endDate);
            var expenseReport = await GetExpenseReport(companyId, startDate, endDate);
            var shiftReport = await GetShiftReport(companyId, startDate, endDate);

            var report = new CompanyReportDto
            {
                EmployeeReport = employeeReport.Value,
                LeaveReport = leaveReport.Value,
                ExpenseReport = expenseReport.Value,
                ShiftReport = shiftReport.Value
            };

            return Ok(report);
        }
    }
}