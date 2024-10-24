using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using System.Security.Claims;

namespace HrManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveService _leaveService;
        private readonly IExpenseService _expenseService;
        private readonly IApplicationUserService _userService;

        public EmployeeController(
            IEmployeeService employeeService,
            ILeaveService leaveService,
            IExpenseService expenseService,
            IApplicationUserService userService)
        {
            _employeeService = employeeService;
            _leaveService = leaveService;
            _expenseService = expenseService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var employees = await _employeeService.GetEmployeesByCompanyAsync(user.CompanyId);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(string id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(Guid.Parse(id));
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeDto employeeDto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            employeeDto.CompanyId = user.CompanyId;

            var createdEmployee = await _employeeService.CreateEmployeeAsync(employeeDto);
            return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto employeeDto)
        {
            if (id != employeeDto.Id)
            {
                return BadRequest();
            }

            var result = await _employeeService.UpdateEmployeeAsync(employeeDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(Guid.Parse(id));
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<PaginatedResultDto<EmployeeDto>>> GetPaginatedEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var paginatedResult = await _employeeService.GetPaginatedEmployeesAsync(user.CompanyId, pageNumber, pageSize);
            return Ok(paginatedResult);
        }

        [HttpGet("{id}/leaves")]
        public async Task<ActionResult<IEnumerable<LeaveDto>>> GetEmployeeLeaves(string id)
        {
            var leaves = await _leaveService.GetLeavesByEmployeeAsync(Guid.Parse(id));
            return Ok(leaves);
        }

        [HttpGet("{id}/expenses")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetEmployeeExpenses(string id)
        {
            var expenses = await _expenseService.GetExpensesByEmployeeAsync(Guid.Parse(id));
            return Ok(expenses);
        }

        [HttpPut("{id}/position")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateEmployeePosition(string id, [FromBody] string newPosition)
        {
            var result = await _employeeService.UpdateEmployeePositionAsync(Guid.Parse(id), newPosition);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("upcoming-birthdays")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetUpcomingBirthdays([FromQuery] int daysAhead = 30)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var employees = await _employeeService.GetUpcomingBirthdaysAsync(user.CompanyId, daysAhead);
            return Ok(employees);
        }

        [HttpGet("{id}/resume")]
        public async Task<ActionResult<ResumeDto>> GetEmployeeResume(string id)
        {
            var resume = await _employeeService.GetEmployeeResumeAsync(Guid.Parse(id));
            if (resume == null)
            {
                return NotFound();
            }
            return Ok(resume);
        }

        [HttpPut("{id}/resume")]
        public async Task<IActionResult> UpdateEmployeeResume(string id, ResumeDto resumeDto)
        {
            var result = await _employeeService.UpdateEmployeeResumeAsync(Guid.Parse(id), resumeDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("current")]
        public async Task<ActionResult<EmployeeDto>> GetCurrentEmployee()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var employee = await _employeeService.GetEmployeeByIdAsync(userId);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employee);
        }
    }
}