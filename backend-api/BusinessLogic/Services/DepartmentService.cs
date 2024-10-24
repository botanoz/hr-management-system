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
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetDepartmentWithEmployeesAsync(departmentId);
                if (department == null)
                {
                    throw new Exception($"Department with ID {departmentId} not found.");
                }
                return _mapper.Map<DepartmentDto>(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting department with ID {departmentId}");
                throw;
            }
        }

        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsByCompanyAsync(int companyId)
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetDepartmentsByCompanyAsync(companyId);
                return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting departments for company with ID {companyId}");
                throw;
            }
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            try
            {
                var department = _mapper.Map<Department>(departmentDto);
                await _unitOfWork.Departments.AddAsync(department);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<DepartmentDto>(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new department");
                throw;
            }
        }

        public async Task<bool> UpdateDepartmentAsync(DepartmentDto departmentDto)
        {
            try
            {
                var existingDepartment = await _unitOfWork.Departments.GetByIdAsync(departmentDto.Id);
                if (existingDepartment == null)
                {
                    throw new Exception($"Department with ID {departmentDto.Id} not found.");
                }

                _mapper.Map(departmentDto, existingDepartment);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating department with ID {departmentDto.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
                if (department == null)
                {
                    throw new Exception($"Department with ID {departmentId} not found.");
                }

                await _unitOfWork.Departments.DeleteAsync(department);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting department with ID {departmentId}");
                throw;
            }
        }

        public async Task<PaginatedResultDto<DepartmentDto>> GetPaginatedDepartmentsAsync(int companyId, int pageNumber, int pageSize)
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetDepartmentsByCompanyAsync(companyId);
                var totalCount = departments.Count();
                var paginatedDepartments = departments
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(paginatedDepartments);

                return new PaginatedResultDto<DepartmentDto>
                {
                    Items = departmentDtos.ToList(),
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting paginated departments for company with ID {companyId}");
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetDepartmentWithEmployeesAsync(departmentId);
                if (department == null)
                {
                    throw new Exception($"Department with ID {departmentId} not found.");
                }
                return _mapper.Map<IEnumerable<EmployeeDto>>(department.Employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting employees for department with ID {departmentId}");
                throw;
            }
        }

        public async Task<bool> AssignEmployeeToDepartmentAsync(Guid employeeId, int departmentId)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    throw new Exception($"Employee with ID {employeeId} not found.");
                }

                var department = await _unitOfWork.Departments.GetByIdAsync(departmentId);
                if (department == null)
                {
                    throw new Exception($"Department with ID {departmentId} not found.");
                }

                employee.DepartmentId = departmentId;
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while assigning employee {employeeId} to department {departmentId}");
                throw;
            }
        }

        public async Task<bool> RemoveEmployeeFromDepartmentAsync(Guid employeeId)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
                if (employee == null)
                {
                    throw new Exception($"Employee with ID {employeeId} not found.");
                }

                employee.DepartmentId = 0; // Assuming 0 represents no department
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while removing employee {employeeId} from their department");
                throw;
            }
        }

        public async Task<DepartmentSummaryDto> GetDepartmentSummaryAsync(int departmentId)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetDepartmentWithEmployeesAsync(departmentId);
                if (department == null)
                {
                    throw new Exception($"Department with ID {departmentId} not found.");
                }

                var employeeCount = department.Employees.Count();
                var pendingLeaveRequests = department.Employees
                    .SelectMany(e => e.Leaves)
                    .Count(l => l.Status == "Pending");

                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                var totalExpensesThisMonth = department.Employees
                    .SelectMany(e => e.Expenses)
                    .Where(ex => ex.ExpenseDate.Month == currentMonth && ex.ExpenseDate.Year == currentYear && ex.Status == "Approved")
                    .Sum(ex => ex.Amount);

                return new DepartmentSummaryDto
                {
                    DepartmentId = department.Id,
                    DepartmentName = department.Name,
                    EmployeeCount = employeeCount,
                    PendingLeaveRequests = pendingLeaveRequests,
                    TotalExpensesThisMonth = totalExpensesThisMonth
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting summary for department with ID {departmentId}");
                throw;
            }
        }
    }
}