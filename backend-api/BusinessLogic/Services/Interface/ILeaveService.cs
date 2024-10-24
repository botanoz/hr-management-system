using HrManagementSystem.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface ILeaveService
    {
        Task<LeaveDto> GetLeaveByIdAsync(int leaveId);
        Task<IEnumerable<LeaveDto>> GetLeavesByEmployeeAsync(Guid employeeId);
        Task<LeaveDto> CreateLeaveAsync(LeaveDto leaveDto);
        Task<bool> UpdateLeaveAsync(LeaveDto leaveDto);
        Task<bool> DeleteLeaveAsync(int leaveId);
        Task<bool> ApproveLeaveAsync(ApprovalDto approvalDto);
        Task<bool> RejectLeaveAsync(ApprovalDto approvalDto);
        Task<int> GetRemainingLeaveDaysAsync(Guid employeeId);
        Task<PaginatedResultDto<LeaveDto>> GetPaginatedLeavesAsync(int companyId, int pageNumber, int pageSize);
    }
}