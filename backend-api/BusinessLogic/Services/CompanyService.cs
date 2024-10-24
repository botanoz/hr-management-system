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
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;

        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CompanyService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CompanyDto> GetCompanyByIdAsync(int companyId)
        {
            try
            {
                var company = await _unitOfWork.Companies.GetByIdAsync(companyId);
                if (company == null)
                {
                    throw new Exception($"Company with ID {companyId} not found.");
                }
                return _mapper.Map<CompanyDto>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting company with ID {companyId}");
                throw;
            }
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync()
        {
            try
            {
                var companies = await _unitOfWork.Companies.GetAllAsync();
                return _mapper.Map<IEnumerable<CompanyDto>>(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all companies");
                throw;
            }
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyDto companyDto)
        {
            try
            {
                var company = _mapper.Map<Company>(companyDto);
                await _unitOfWork.Companies.AddAsync(company);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<CompanyDto>(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new company");
                throw;
            }
        }

        public async Task<bool> UpdateCompanyAsync(CompanyDto companyDto)
        {
            try
            {
                var existingCompany = await _unitOfWork.Companies.GetByIdAsync(companyDto.Id);
                if (existingCompany == null)
                {
                    throw new Exception($"Company with ID {companyDto.Id} not found.");
                }

                _mapper.Map(companyDto, existingCompany);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating company with ID {companyDto.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteCompanyAsync(int companyId)
        {
            try
            {
                var company = await _unitOfWork.Companies.GetByIdAsync(companyId);
                if (company == null)
                {
                    throw new Exception($"Company with ID {companyId} not found.");
                }

                await _unitOfWork.Companies.DeleteAsync(company);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting company with ID {companyId}");
                throw;
            }
        }

        public async Task<bool> ApproveCompanyAsync(int companyId)
        {
            try
            {
                var company = await _unitOfWork.Companies.GetByIdAsync(companyId);
                if (company == null)
                {
                    throw new Exception($"Company with ID {companyId} not found.");
                }

                company.IsApproved = true;
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while approving company with ID {companyId}");
                throw;
            }
        }

        public async Task<bool> RejectCompanyAsync(int companyId, string reason)
        {
            try
            {
                var company = await _unitOfWork.Companies.GetByIdAsync(companyId);
                if (company == null)
                {
                    throw new Exception($"Company with ID {companyId} not found.");
                }

                company.IsApproved = false;
                // Assuming there's a RejectionReason property in the Company entity
               
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while rejecting company with ID {companyId}");
                throw;
            }
        }

        public async Task<bool> UpdateCompanySubscriptionAsync(int companyId, DateTime newEndDate)
        {
            try
            {
                var company = await _unitOfWork.Companies.GetByIdAsync(companyId);
                if (company == null)
                {
                    throw new Exception($"Company with ID {companyId} not found.");
                }

                company.SubscriptionEndDate = newEndDate;
                await _unitOfWork.CompleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating subscription for company with ID {companyId}");
                throw;
            }
        }

        public async Task<PaginatedResultDto<CompanyDto>> GetPaginatedCompaniesAsync(int pageNumber, int pageSize)
        {
            try
            {
                var companies = await _unitOfWork.Companies.GetAllAsync();
                var totalCount = companies.Count();
                var paginatedCompanies = companies
                    .OrderBy(c => c.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(paginatedCompanies);

                return new PaginatedResultDto<CompanyDto>
                {
                    Items = companyDtos.ToList(),
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting paginated companies");
                throw;
            }
        }
    }
}