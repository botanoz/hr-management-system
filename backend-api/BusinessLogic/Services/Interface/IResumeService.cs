using HrManagementSystem.BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IResumeService
    {
        Task<ResumeDto> GetResumeByIdAsync(int id);
        Task<ResumeDto> GetResumeByEmployeeIdAsync(Guid employeeId);
        Task<ResumeDto> CreateResumeAsync(ResumeDto resumeDto);
        Task UpdateResumeAsync(ResumeDto resumeDto);
        Task DeleteResumeAsync(int id);
    }
}