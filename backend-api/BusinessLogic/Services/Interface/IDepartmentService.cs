using HrManagementSystem.BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IDepartmentService
    {
        Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId);
        Task<IEnumerable<DepartmentDto>> GetDepartmentsByCompanyAsync(int companyId);
        Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto departmentDto);
        Task<bool> UpdateDepartmentAsync(DepartmentDto departmentDto);
        Task<bool> DeleteDepartmentAsync(int departmentId);
        Task<bool> AssignEmployeeToDepartmentAsync(Guid employeeId, int departmentId);
    }
}