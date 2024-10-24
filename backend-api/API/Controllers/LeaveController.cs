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
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly IApplicationUserService _userService;

        public LeaveController(ILeaveService leaveService, IApplicationUserService userService)
        {
            _leaveService = leaveService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveDto>>> GetLeavesByEmployee()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var leaves = await _leaveService.GetLeavesByEmployeeAsync(userId);
            return Ok(leaves);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveDto>> GetLeave(int id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return Ok(leave);
        }

        [HttpPost]
        public async Task<ActionResult<LeaveDto>> CreateLeave(LeaveDto leaveDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            leaveDto.EmployeeId = Guid.Parse(userId);
            var createdLeave = await _leaveService.CreateLeaveAsync(leaveDto);
            return CreatedAtAction(nameof(GetLeave), new { id = createdLeave.Id }, createdLeave);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeave(int id, LeaveDto leaveDto)
        {
            if (id != leaveDto.Id)
            {
                return BadRequest();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (leaveDto.EmployeeId != Guid.Parse(userId))
            {
                return Forbid();
            }

            var result = await _leaveService.UpdateLeaveAsync(leaveDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var leave = await _leaveService.GetLeaveByIdAsync(id);

            if (leave == null)
            {
                return NotFound();
            }

            if (leave.EmployeeId != Guid.Parse(userId))
            {
                return Forbid();
            }

            var result = await _leaveService.DeleteLeaveAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ApproveLeave(int id, ApprovalDto approvalDto)
        {
            approvalDto.Id = id;
            var result = await _leaveService.ApproveLeaveAsync(approvalDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> RejectLeave(int id, ApprovalDto approvalDto)
        {
            approvalDto.Id = id;
            var result = await _leaveService.RejectLeaveAsync(approvalDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("remaining-days")]
        public async Task<ActionResult<int>> GetRemainingLeaveDays()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var remainingDays = await _leaveService.GetRemainingLeaveDaysAsync(userId);
            return Ok(remainingDays);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<PaginatedResultDto<LeaveDto>>> GetPaginatedLeaves([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var paginatedResult = await _leaveService.GetPaginatedLeavesAsync(user.CompanyId, pageNumber, pageSize);
            return Ok(paginatedResult);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<LeaveDto>>> GetPendingLeaves()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var paginatedResult = await _leaveService.GetPaginatedLeavesAsync(user.CompanyId, 1, int.MaxValue);
            var pendingLeaves = paginatedResult.Items.FindAll(l => l.Status == "Pending");
            return Ok(pendingLeaves);
        }
    }
}