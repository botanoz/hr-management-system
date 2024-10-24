using HrManagementSystem.BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface ICompanyService
    {
        Task<CompanyDto> GetCompanyByIdAsync(int companyId);
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
        Task<CompanyDto> CreateCompanyAsync(CompanyDto companyDto);
        Task<bool> UpdateCompanyAsync(CompanyDto companyDto);
        Task<bool> DeleteCompanyAsync(int companyId);
        Task<bool> ApproveCompanyAsync(int companyId);
        Task<bool> RejectCompanyAsync(int companyId, string reason);
        Task<bool> UpdateCompanySubscriptionAsync(int companyId, DateTime newEndDate);
        Task<PaginatedResultDto<CompanyDto>> GetPaginatedCompaniesAsync(int pageNumber, int pageSize);
    }
}