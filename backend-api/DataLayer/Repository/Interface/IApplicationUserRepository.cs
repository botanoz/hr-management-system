using HrManagementSystem.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories.Interface
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetUserWithDetailsAsync(Guid userId);
        Task<IEnumerable<ApplicationUser>> GetUsersByCompanyAsync(int companyId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
    }
}