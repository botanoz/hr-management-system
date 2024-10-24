using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;

namespace HrManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        private readonly IEmployeeService _employeeService;

        public ResumeController(IResumeService resumeService, IEmployeeService employeeService)
        {
            _resumeService = resumeService;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<ResumeDto>> GetMyResume()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var employee = await _employeeService.GetEmployeeByIdAsync(userId);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            var resume = await _resumeService.GetResumeByEmployeeIdAsync(employee.EmployeeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            return Ok(resume);
        }

        [HttpGet("{employeeId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ResumeDto>> GetEmployeeResume(string employeeId)
        {
            var resume = await _resumeService.GetResumeByEmployeeIdAsync(Guid.Parse(employeeId));
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            return Ok(resume);
        }

        [HttpPost]
        public async Task<ActionResult<ResumeDto>> CreateResume(ResumeDto resumeDto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var employee = await _employeeService.GetEmployeeByIdAsync(userId);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            resumeDto.EmployeeId = employee.EmployeeId;
            var createdResume = await _resumeService.CreateResumeAsync(resumeDto);
            return CreatedAtAction(nameof(GetMyResume), new { }, createdResume);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateResume(ResumeDto resumeDto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var employee = await _employeeService.GetEmployeeByIdAsync(userId);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            if (employee.EmployeeId != resumeDto.EmployeeId)
            {
                return Forbid();
            }

            await _resumeService.UpdateResumeAsync(resumeDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResume(int id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var employee = await _employeeService.GetEmployeeByIdAsync(userId);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            var resume = await _resumeService.GetResumeByIdAsync(id);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            if (resume.EmployeeId != employee.EmployeeId)
            {
                return Forbid();
            }

            await _resumeService.DeleteResumeAsync(id);
            return NoContent();
        }

        // Education
        [HttpPost("{resumeId}/education")]
        public async Task<ActionResult<ResumeDto>> AddEducation(int resumeId, EducationDto educationDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            resume.Educations.Add(educationDto);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpPut("{resumeId}/education/{educationId}")]
        public async Task<ActionResult<ResumeDto>> UpdateEducation(int resumeId, int educationId, EducationDto educationDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var educationToUpdate = resume.Educations.Find(e => e.Id == educationId);
            if (educationToUpdate == null)
            {
                return NotFound("Education not found");
            }

            // Update education properties
            educationToUpdate.SchoolName = educationDto.SchoolName;
            educationToUpdate.Degree = educationDto.Degree;
            educationToUpdate.StartDate = educationDto.StartDate;
            educationToUpdate.EndDate = educationDto.EndDate;
            educationToUpdate.FieldOfStudy = educationDto.FieldOfStudy;

            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpDelete("{resumeId}/education/{educationId}")]
        public async Task<ActionResult<ResumeDto>> DeleteEducation(int resumeId, int educationId)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var educationToRemove = resume.Educations.Find(e => e.Id == educationId);
            if (educationToRemove == null)
            {
                return NotFound("Education not found");
            }

            resume.Educations.Remove(educationToRemove);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        // Work Experience
        [HttpPost("{resumeId}/workexperience")]
        public async Task<ActionResult<ResumeDto>> AddWorkExperience(int resumeId, WorkExperienceDto workExperienceDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            resume.WorkExperiences.Add(workExperienceDto);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpPut("{resumeId}/workexperience/{workExperienceId}")]
        public async Task<ActionResult<ResumeDto>> UpdateWorkExperience(int resumeId, int workExperienceId, WorkExperienceDto workExperienceDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var workExperienceToUpdate = resume.WorkExperiences.Find(w => w.Id == workExperienceId);
            if (workExperienceToUpdate == null)
            {
                return NotFound("Work experience not found");
            }

            // Update work experience properties
            workExperienceToUpdate.CompanyName = workExperienceDto.CompanyName;
            workExperienceToUpdate.Position = workExperienceDto.Position;
            workExperienceToUpdate.StartDate = workExperienceDto.StartDate;
            workExperienceToUpdate.EndDate = workExperienceDto.EndDate;
            workExperienceToUpdate.Description = workExperienceDto.Description;

            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpDelete("{resumeId}/workexperience/{workExperienceId}")]
        public async Task<ActionResult<ResumeDto>> DeleteWorkExperience(int resumeId, int workExperienceId)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var workExperienceToRemove = resume.WorkExperiences.Find(w => w.Id == workExperienceId);
            if (workExperienceToRemove == null)
            {
                return NotFound("Work experience not found");
            }

            resume.WorkExperiences.Remove(workExperienceToRemove);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        // Skill methods
        [HttpPost("{resumeId}/skill")]
        public async Task<ActionResult<ResumeDto>> AddSkill(int resumeId, SkillDto skillDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            resume.Skills.Add(skillDto);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpPut("{resumeId}/skill/{skillId}")]
        public async Task<ActionResult<ResumeDto>> UpdateSkill(int resumeId, int skillId, SkillDto skillDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var skillToUpdate = resume.Skills.Find(s => s.Id == skillId);
            if (skillToUpdate == null)
            {
                return NotFound("Skill not found");
            }

            skillToUpdate.Name = skillDto.Name;
            skillToUpdate.Proficiency = skillDto.Proficiency;

            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpDelete("{resumeId}/skill/{skillId}")]
        public async Task<ActionResult<ResumeDto>> DeleteSkill(int resumeId, int skillId)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var skillToRemove = resume.Skills.Find(s => s.Id == skillId);
            if (skillToRemove == null)
            {
                return NotFound("Skill not found");
            }

            resume.Skills.Remove(skillToRemove);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        // Certification methods
        [HttpPost("{resumeId}/certification")]
        public async Task<ActionResult<ResumeDto>> AddCertification(int resumeId, CertificationDto certificationDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            resume.Certifications.Add(certificationDto);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpPut("{resumeId}/certification/{certificationId}")]
        public async Task<ActionResult<ResumeDto>> UpdateCertification(int resumeId, int certificationId, CertificationDto certificationDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var certificationToUpdate = resume.Certifications.Find(c => c.Id == certificationId);
            if (certificationToUpdate == null)
            {
                return NotFound("Certification not found");
            }

            certificationToUpdate.Name = certificationDto.Name;
            certificationToUpdate.Issuer = certificationDto.Issuer;
            certificationToUpdate.DateIssued = certificationDto.DateIssued;

            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpDelete("{resumeId}/certification/{certificationId}")]
        public async Task<ActionResult<ResumeDto>> DeleteCertification(int resumeId, int certificationId)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var certificationToRemove = resume.Certifications.Find(c => c.Id == certificationId);
            if (certificationToRemove == null)
            {
                return NotFound("Certification not found");
            }

            resume.Certifications.Remove(certificationToRemove);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        // Language methods
        [HttpPost("{resumeId}/language")]
        public async Task<ActionResult<ResumeDto>> AddLanguage(int resumeId, LanguageDto languageDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            resume.Languages.Add(languageDto);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpPut("{resumeId}/language/{languageId}")]
        public async Task<ActionResult<ResumeDto>> UpdateLanguage(int resumeId, int languageId, LanguageDto languageDto)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var languageToUpdate = resume.Languages.Find(l => l.Id == languageId);
            if (languageToUpdate == null)
            {
                return NotFound("Language not found");
            }

            languageToUpdate.Name = languageDto.Name;
            languageToUpdate.Proficiency = languageDto.Proficiency;

            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }

        [HttpDelete("{resumeId}/language/{languageId}")]
        public async Task<ActionResult<ResumeDto>> DeleteLanguage(int resumeId, int languageId)
        {
            var resume = await _resumeService.GetResumeByIdAsync(resumeId);
            if (resume == null)
            {
                return NotFound("Resume not found");
            }

            var languageToRemove = resume.Languages.Find(l => l.Id == languageId);
            if (languageToRemove == null)
            {
                return NotFound("Language not found");
            }

            resume.Languages.Remove(languageToRemove);
            await _resumeService.UpdateResumeAsync(resume);
            return Ok(resume);
        }
    }
}