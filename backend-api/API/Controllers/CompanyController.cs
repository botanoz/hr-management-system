using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;

namespace HrManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IApplicationUserService _userService;

        public CompanyController(ICompanyService companyService, IApplicationUserService userService)
        {
            _companyService = companyService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyDto companyDto)
        {
            var createdCompany = await _companyService.CreateCompanyAsync(companyDto);
            return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyDto companyDto)
        {
            if (id != companyDto.Id)
            {
                return BadRequest();
            }

            var result = await _companyService.UpdateCompanyAsync(companyDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveCompany(int id)
        {
            var result = await _companyService.ApproveCompanyAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectCompany(int id, [FromBody] string reason)
        {
            var result = await _companyService.RejectCompanyAsync(id, reason);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}/subscription")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCompanySubscription(int id, [FromBody] DateTime newEndDate)
        {
            var result = await _companyService.UpdateCompanySubscriptionAsync(id, newEndDate);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PaginatedResultDto<CompanyDto>>> GetPaginatedCompanies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var paginatedResult = await _companyService.GetPaginatedCompaniesAsync(pageNumber, pageSize);
            return Ok(paginatedResult);
        }

        [HttpGet("summary")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CompanySummaryDto>>> GetCompanySummaries()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            var summaries = new List<CompanySummaryDto>();
            foreach (var company in companies)
            {
                summaries.Add(new CompanySummaryDto
                {
                    CompanyId = company.Id,
                    CompanyName = company.Name,
                    RegistrationDate = company.RegistrationDate,
                    EmployeeCount = company.EmployeeCount,
                    SubscriptionEndDate = company.SubscriptionEndDate
                });
            }
            return Ok(summaries);
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<ActionResult<CompanyDto>> GetCurrentUserCompany()
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var company = await _companyService.GetCompanyByIdAsync(user.CompanyId);
            if (company == null)
            {
                return NotFound("Company not found");
            }

            return Ok(company);
        }
    }
}