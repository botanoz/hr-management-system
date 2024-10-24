using HrManagementSystem.BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId);
        Task<IEnumerable<EmployeeDto>> GetEmployeesByCompanyAsync(int companyId);
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto);
        Task<bool> UpdateEmployeeAsync(EmployeeDto employeeDto);
        Task<bool> DeleteEmployeeAsync(Guid employeeId);
        Task<IEnumerable<EmployeeDto>> GetUpcomingBirthdaysAsync(int companyId, int daysAhead);
        Task<bool> UpdateEmployeePositionAsync(Guid employeeId, string newPosition);
        Task<ResumeDto> GetEmployeeResumeAsync(Guid employeeId);
        Task<bool> UpdateEmployeeResumeAsync(Guid employeeId, ResumeDto resumeDto);
        Task<PaginatedResultDto<EmployeeDto>> GetPaginatedEmployeesAsync(int companyId, int pageNumber, int pageSize);
        Task<EmployeeDto> GetEmployeeByEmailAsync(string email);
    }
}