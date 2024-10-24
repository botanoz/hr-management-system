using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;

namespace HrManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IApplicationUserService _userService;

        public ExpenseController(IExpenseService expenseService, IApplicationUserService userService)
        {
            _expenseService = expenseService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByEmployee()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var expenses = await _expenseService.GetExpensesByEmployeeAsync(userId);
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> CreateExpense(ExpenseDto expenseDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            expenseDto.EmployeeId = Guid.Parse(userId);
            var createdExpense = await _expenseService.CreateExpenseAsync(expenseDto);
            return CreatedAtAction(nameof(GetExpense), new { id = createdExpense.Id }, createdExpense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, ExpenseDto expenseDto)
        {
            if (id != expenseDto.Id)
            {
                return BadRequest();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (expenseDto.EmployeeId != Guid.Parse(userId))
            {
                return Forbid();
            }

            var result = await _expenseService.UpdateExpenseAsync(expenseDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var expense = await _expenseService.GetExpenseByIdAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            if (expense.EmployeeId != Guid.Parse(userId))
            {
                return Forbid();
            }

            var result = await _expenseService.DeleteExpenseAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ApproveExpense(int id, ApprovalDto approvalDto)
        {
            approvalDto.Id = id;
            var result = await _expenseService.ApproveExpenseAsync(approvalDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> RejectExpense(int id, ApprovalDto approvalDto)
        {
            approvalDto.Id = id;
            var result = await _expenseService.RejectExpenseAsync(approvalDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<PaginatedResultDto<ExpenseDto>>> GetPaginatedExpenses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var paginatedResult = await _expenseService.GetPaginatedExpensesAsync(user.CompanyId, pageNumber, pageSize);
            return Ok(paginatedResult);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetPendingExpenses()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var paginatedResult = await _expenseService.GetPaginatedExpensesAsync(user.CompanyId, 1, int.MaxValue);
            var pendingExpenses = paginatedResult.Items.FindAll(e => e.Status == "Pending");
            return Ok(pendingExpenses);
        }

        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetTotalExpensesByEmployee([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var total = await _expenseService.GetTotalExpensesByEmployeeAsync(userId, startDate, endDate);
            return Ok(total);
        }

        [HttpGet("weekly-total")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<decimal>> GetWeeklyExpenseTotal()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);

            var startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).Date;
            var endDate = startDate.AddDays(7).AddSeconds(-1);

            var paginatedResult = await _expenseService.GetPaginatedExpensesAsync(user.CompanyId, 1, int.MaxValue);
            var weeklyExpenses = paginatedResult.Items
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate && e.Status == "Approved")
                .Sum(e => e.Amount);

            return Ok(weeklyExpenses);
        }

        // Yeni eklenen metod
        [HttpGet("monthly-total")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<decimal>> GetMonthlyExpenseTotal()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);

            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var paginatedResult = await _expenseService.GetPaginatedExpensesAsync(user.CompanyId, 1, int.MaxValue);
            var monthlyExpenses = paginatedResult.Items
                .Where(e => e.ExpenseDate >= startDate && e.ExpenseDate <= endDate && e.Status == "Approved")
                .Sum(e => e.Amount);

            return Ok(monthlyExpenses);
        }
    }
}