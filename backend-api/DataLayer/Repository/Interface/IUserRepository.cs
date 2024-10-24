using HrManagementSystem.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories.Interface
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetUserWithDetailsAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetUsersByCompanyAsync(int companyId);
        Task<string> GetUserRoleAsync(string userId);
        Task<bool> SetUserRoleAsync(string userId, string roleName);
    }
}
