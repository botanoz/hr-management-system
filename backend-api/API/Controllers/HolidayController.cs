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
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;
        private readonly IApplicationUserService _userService;

        public HolidayController(IHolidayService holidayService, IApplicationUserService userService)
        {
            _holidayService = holidayService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HolidayDto>> GetHoliday(int id)
        {
            var holiday = await _holidayService.GetHolidayByIdAsync(id);
            if (holiday == null)
            {
                return NotFound();
            }
            return Ok(holiday);
        }

        [HttpGet("company")]
        public async Task<ActionResult<IEnumerable<HolidayDto>>> GetHolidaysByCompany()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var holidays = await _holidayService.GetHolidaysByCompanyAsync(user.CompanyId);
            return Ok(holidays);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<HolidayDto>> CreateHoliday(HolidayDto holidayDto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            holidayDto.CompanyId = user.CompanyId;

            var createdHoliday = await _holidayService.CreateHolidayAsync(holidayDto);
            return CreatedAtAction(nameof(GetHoliday), new { id = createdHoliday.Id }, createdHoliday);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateHoliday(int id, HolidayDto holidayDto)
        {
            if (id != holidayDto.Id)
            {
                return BadRequest();
            }

            var result = await _holidayService.UpdateHolidayAsync(holidayDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteHoliday(int id)
        {
            var result = await _holidayService.DeleteHolidayAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<HolidayDto>>> GetUpcomingHolidays([FromQuery] int daysAhead = 30)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            var upcomingHolidays = await _holidayService.GetUpcomingHolidaysAsync(user.CompanyId, daysAhead);
            return Ok(upcomingHolidays);
        }
    }
}