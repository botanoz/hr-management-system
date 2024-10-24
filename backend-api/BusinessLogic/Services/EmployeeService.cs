using AutoMapper;
using HrManagementSystem.BusinessLogic.DTOs;

using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId)
        {
            var employee = await _unitOfWork.Employees.GetEmployeeWithFullDetailsAsync(employeeId);
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByCompanyAsync(int companyId)
        {
            var employees = await _unitOfWork.Employees.GetEmployeesByCompanyAsync(companyId);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeDto employeeDto)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeDto.Id);
            if (employee == null) return false;
            _mapper.Map(employeeDto, employee);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(Guid employeeId)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
            if (employee == null) return false;
            await _unitOfWork.Employees.DeleteAsync(employee);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeDto>> GetUpcomingBirthdaysAsync(int companyId, int daysAhead)
        {
            var employees = await _unitOfWork.Employees.GetUpcomingBirthdaysAsync(companyId, daysAhead);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<bool> UpdateEmployeePositionAsync(Guid employeeId, string newPosition)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
            if (employee == null) return false;
            employee.Position = newPosition;
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<ResumeDto> GetEmployeeResumeAsync(Guid employeeId)
        {
            var resume = await _unitOfWork.Resumes.GetResumeWithDetailsAsync(employeeId);
            return _mapper.Map<ResumeDto>(resume);
        }

        public async Task<bool> UpdateEmployeeResumeAsync(Guid employeeId, ResumeDto resumeDto)
        {
            var resume = await _unitOfWork.Resumes.GetByIdAsync(resumeDto.Id);
            if (resume == null || resume.EmployeeId != employeeId) return false;
            _mapper.Map(resumeDto, resume);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<PaginatedResultDto<EmployeeDto>> GetPaginatedEmployeesAsync(int companyId, int pageNumber, int pageSize)
        {
            var employees = await _unitOfWork.Employees.GetEmployeesByCompanyAsync(companyId);
            var totalCount = employees.Count();
            var paginatedEmployees = employees.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(paginatedEmployees);

            return new PaginatedResultDto<EmployeeDto>
            {
                Items = employeeDtos.ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<EmployeeDto> GetEmployeeByEmailAsync(string email)
    {
        var employee = await _unitOfWork.Employees.GetEmployeeByEmailAsync(email);
        return _mapper.Map<EmployeeDto>(employee);
    }
    }
}