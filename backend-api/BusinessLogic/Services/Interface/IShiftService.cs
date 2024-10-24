using HrManagementSystem.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IShiftService
    {
        Task<ShiftDto> GetShiftByIdAsync(Guid shiftId);
        Task<IEnumerable<ShiftDto>> GetShiftsByEmployeeAsync(Guid employeeId);
        Task<ShiftDto> CreateShiftAsync(ShiftDto shiftDto);
        Task<bool> UpdateShiftAsync(ShiftDto shiftDto);
        Task<bool> DeleteShiftAsync(Guid shiftId);
        Task<int> GetWeeklyWorkHoursAsync(Guid employeeId, DateTime weekStartDate);
        Task<PaginatedResultDto<ShiftDto>> GetPaginatedShiftsAsync(int companyId, int pageNumber, int pageSize);
    }
}