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
    public class ShiftService : IShiftService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ShiftService> _logger;

        public ShiftService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ShiftService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ShiftDto> GetShiftByIdAsync(Guid shiftId)
        {
            try
            {
                var shift = await _unitOfWork.Shifts.GetByIdAsync(shiftId);
                if (shift == null)
                {
                    throw new Exception($"Shift with ID {shiftId} not found.");
                }
                return _mapper.Map<ShiftDto>(shift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting shift with ID {shiftId}");
                throw;
            }
        }

        public async Task<IEnumerable<ShiftDto>> GetShiftsByEmployeeAsync(Guid employeeId)
        {
            try
            {
                var shifts = await _unitOfWork.Shifts.GetShiftsByEmployeeAsync(employeeId);
                return _mapper.Map<IEnumerable<ShiftDto>>(shifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting shifts for employee with ID {employeeId}");
                throw;
            }
        }

        public async Task<ShiftDto> CreateShiftAsync(ShiftDto shiftDto)
        {
            try
            {
                var shift = _mapper.Map<Shift>(shiftDto);
                await _unitOfWork.Shifts.AddAsync(shift);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<ShiftDto>(shift);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new shift");
                throw;
            }
        }

        public async Task<bool> UpdateShiftAsync(ShiftDto shiftDto)
        {
            try
            {
                var existingShift = await _unitOfWork.Shifts.GetByIdAsync(shiftDto.Id);
                if (existingShift == null)
                {
                    throw new Exception($"Shift with ID {shiftDto.Id} not found.");
                }

                _mapper.Map(shiftDto, existingShift);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating shift with ID {shiftDto.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteShiftAsync(Guid shiftId)
        {
            try
            {
                var shift = await _unitOfWork.Shifts.GetByIdAsync(shiftId);
                if (shift == null)
                {
                    throw new Exception($"Shift with ID {shiftId} not found.");
                }

                await _unitOfWork.Shifts.DeleteAsync(shift);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting shift with ID {shiftId}");
                throw;
            }
        }

        public async Task<int> GetWeeklyWorkHoursAsync(Guid employeeId, DateTime weekStartDate)
        {
            try
            {
                var weekEndDate = weekStartDate.AddDays(7);
                var shifts = await _unitOfWork.Shifts.GetShiftsByEmployeeAsync(employeeId);
                var weeklyShifts = shifts.Where(s => s.StartTime >= weekStartDate && s.EndTime <= weekEndDate);

                int totalMinutes = weeklyShifts.Sum(s => (int)(s.EndTime - s.StartTime).TotalMinutes);
                return totalMinutes / 60; // Convert minutes to hours
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while calculating weekly work hours for employee {employeeId}");
                throw;
            }
        }

        public async Task<PaginatedResultDto<ShiftDto>> GetPaginatedShiftsAsync(int companyId, int pageNumber, int pageSize)
        {
            try
            {
                var allShifts = await _unitOfWork.Shifts.GetAllAsync();
                var companyShifts = allShifts.Where(s => s.Employee.CompanyId == companyId);

                var totalCount = companyShifts.Count();
                var paginatedShifts = companyShifts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var shiftDtos = _mapper.Map<IEnumerable<ShiftDto>>(paginatedShifts);

                return new PaginatedResultDto<ShiftDto>
                {
                    Items = shiftDtos.ToList(),
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting paginated shifts for company {companyId}");
                throw;
            }
        }
    }
}