using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;

namespace HrManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        private readonly IApplicationUserService _userService;

        public ShiftController(IShiftService shiftService, IApplicationUserService userService)
        {
            _shiftService = shiftService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftDto>>> GetShiftsByEmployee()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var shifts = await _shiftService.GetShiftsByEmployeeAsync(userId);
            return Ok(shifts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShiftDto>> GetShift(string id)
        {
            var shift = await _shiftService.GetShiftByIdAsync(Guid.Parse(id));
            if (shift == null)
            {
                return NotFound();
            }
            return Ok(shift);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ShiftDto>> CreateShift(ShiftDto shiftDto)
        {
            var createdShift = await _shiftService.CreateShiftAsync(shiftDto);
            return CreatedAtAction(nameof(GetShift), new { id = createdShift.Id }, createdShift);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateShift(int id, ShiftDto shiftDto)
        {
            if (id != shiftDto.Id)
            {
                return BadRequest();
            }

            var result = await _shiftService.UpdateShiftAsync(shiftDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteShift(Guid id)
        {
            var result = await _shiftService.DeleteShiftAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<ShiftDto>>> GetShiftsByEmployeeId(string employeeId)
        {
            var shifts = await _shiftService.GetShiftsByEmployeeAsync(Guid.Parse(employeeId));
            return Ok(shifts);
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<ShiftDto>>> GetUpcomingShifts()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var shifts = await _shiftService.GetShiftsByEmployeeAsync(userId);
            var upcomingShifts = shifts.Where(s => s.StartTime > DateTime.Now).OrderBy(s => s.StartTime);
            return Ok(upcomingShifts);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<PaginatedResultDto<ShiftDto>>> GetPaginatedShifts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var paginatedResult = await _shiftService.GetPaginatedShiftsAsync(user.CompanyId, pageNumber, pageSize);
            return Ok(paginatedResult);
        }

        [HttpGet("active")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<ShiftDto>>> GetActiveShifts()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var allShifts = await _shiftService.GetPaginatedShiftsAsync(user.CompanyId, 1, int.MaxValue);
            var activeShifts = allShifts.Items.Where(s => s.StartTime <= DateTime.Now && s.EndTime >= DateTime.Now);
            return Ok(activeShifts);
        }

        [HttpPut("assign/{shiftId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AssignEmployeeToShift(Guid shiftId, [FromBody] Guid employeeId)
        {
            var shift = await _shiftService.GetShiftByIdAsync(shiftId);
            if (shift == null)
            {
                return NotFound("Shift not found");
            }

            shift.EmployeeId = employeeId;
            var result = await _shiftService.UpdateShiftAsync(shift);
            if (!result)
            {
                return BadRequest("Failed to assign employee to shift");
            }
            return NoContent();
        }

        [HttpPut("unassign/{shiftId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UnassignEmployeeFromShift(Guid shiftId)
        {
            var shift = await _shiftService.GetShiftByIdAsync(shiftId);
            if (shift == null)
            {
                return NotFound("Shift not found");
            }

            shift.EmployeeId = Guid.Parse("00000000-0000-0000-0000-000000000000"); // Assuming 0 means unassigned
            var result = await _shiftService.UpdateShiftAsync(shift);
            if (!result)
            {
                return BadRequest("Failed to unassign employee from shift");
            }
            return NoContent();
        }
    }
}