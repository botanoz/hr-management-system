using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace HrManagementSystem.BusinessLogic.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LeaveService> _logger;

        public LeaveService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LeaveService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LeaveDto> GetLeaveByIdAsync(int leaveId)
        {
            try
            {
                var leave = await _unitOfWork.Leaves.GetByIdAsync(leaveId);
                if (leave == null)
                {
                    throw new Exception($"Leave with ID {leaveId} not found.");
                }
                return _mapper.Map<LeaveDto>(leave);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting leave with ID {leaveId}");
                throw;
            }
        }

        public async Task<IEnumerable<LeaveDto>> GetLeavesByEmployeeAsync(Guid employeeId)
        {
            try
            {
                var leaves = await _unitOfWork.Leaves.GetLeavesByEmployeeAsync(employeeId);
                return _mapper.Map<IEnumerable<LeaveDto>>(leaves);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting leaves for employee with ID {employeeId}");
                throw;
            }
        }

        public async Task<LeaveDto> CreateLeaveAsync(LeaveDto leaveDto)
        {
            try
            {
                var leave = _mapper.Map<Leave>(leaveDto);
                await _unitOfWork.Leaves.AddAsync(leave);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<LeaveDto>(leave);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new leave");
                throw;
            }
        }

        public async Task<bool> UpdateLeaveAsync(LeaveDto leaveDto)
        {
            try
            {
                var existingLeave = await _unitOfWork.Leaves.GetByIdAsync(leaveDto.Id);
                if (existingLeave == null)
                {
                    throw new Exception($"Leave with ID {leaveDto.Id} not found.");
                }

                _mapper.Map(leaveDto, existingLeave);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating leave with ID {leaveDto.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteLeaveAsync(int leaveId)
        {
            try
            {
                var leave = await _unitOfWork.Leaves.GetByIdAsync(leaveId);
                if (leave == null)
                {
                    throw new Exception($"Leave with ID {leaveId} not found.");
                }

                await _unitOfWork.Leaves.DeleteAsync(leave);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting leave with ID {leaveId}");
                throw;
            }
        }

        public async Task<bool> ApproveLeaveAsync(ApprovalDto approvalDto)
        {
            try
            {
                var leave = await _unitOfWork.Leaves.GetByIdAsync(approvalDto.Id);
                if (leave == null)
                {
                    throw new Exception($"Leave with ID {approvalDto.Id} not found.");
                }

                leave.Status = "Approved";
                leave.Comments = approvalDto.Comments;
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while approving leave with ID {approvalDto.Id}");
                throw;
            }
        }

        public async Task<bool> RejectLeaveAsync(ApprovalDto approvalDto)
        {
            try
            {
                var leave = await _unitOfWork.Leaves.GetByIdAsync(approvalDto.Id);
                if (leave == null)
                {
                    throw new Exception($"Leave with ID {approvalDto.Id} not found.");
                }

                leave.Status = "Rejected";
                leave.Comments = approvalDto.Comments;
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while rejecting leave with ID {approvalDto.Id}");
                throw;
            }
        }

        public async Task<int> GetRemainingLeaveDaysAsync(Guid employeeId)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    throw new Exception($"Employee with ID {employeeId} not found.");
                }

                var leaves = await _unitOfWork.Leaves.GetLeavesByEmployeeAsync(employeeId);
                var approvedLeaves = leaves.Where(l => l.Status == "Approved");

                int totalLeaveDays = approvedLeaves.Sum(l => (l.EndDate - l.StartDate).Days + 1);

                // Assuming there's a standard number of leave days per year, e.g., 20
                const int standardLeaveDays = 20;
                return Math.Max(0, standardLeaveDays - totalLeaveDays);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while calculating remaining leave days for employee {employeeId}");
                throw;
            }
        }

        public async Task<PaginatedResultDto<LeaveDto>> GetPaginatedLeavesAsync(int companyId, int pageNumber, int pageSize)
        {
            try
            {
                var allLeaves = await _unitOfWork.Leaves.GetAllAsync();

                // Filter company leaves, handling possible null Employee references
                var companyLeaves = allLeaves
                    .Where(l => l.Employee?.CompanyId == companyId);

                var totalCount = companyLeaves.Count();
                var paginatedLeaves = companyLeaves
                    .OrderByDescending(l => l.StartDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var leaveDtos = _mapper.Map<IEnumerable<LeaveDto>>(paginatedLeaves);

                return new PaginatedResultDto<LeaveDto>
                {
                    Items = leaveDtos.ToList(),
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting paginated leaves for company {companyId}");
                throw;
            }
        }

    }
}