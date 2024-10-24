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
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Department>> GetDepartmentsByCompanyAsync(int companyId)
        {
            return await _context.Departments
                .Where(d => d.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Department> GetDepartmentWithEmployeesAsync(int departmentId)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == departmentId);
        }
    }
}
