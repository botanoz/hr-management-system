using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context) { }

        public async Task<Company> GetCompanyWithEmployeesAsync(int companyId)
        {
            return await _context.Companies
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesWithDetailsAsync()
        {
            return await _context.Companies
                .Include(c => c.Employees)
                .Include(c => c.Departments)
                .ToListAsync();
        }
    }
}
