using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Employee>> GetEmployeesByCompanyAsync(int companyId)
        {
            return await _context.Employees
                .Where(e => e.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetUpcomingBirthdaysAsync(int companyId, int daysAhead)
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(daysAhead);
            return await _context.Employees
                .Where(e => e.CompanyId == companyId &&
                    ((e.Birthdate.Month == today.Month && e.Birthdate.Day >= today.Day) ||
                     (e.Birthdate.Month == futureDate.Month && e.Birthdate.Day <= futureDate.Day) ||
                     (e.Birthdate.Month > today.Month && e.Birthdate.Month < futureDate.Month)))
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeWithFullDetailsAsync(Guid employeeId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Company)
                .Include(e => e.Resume)
                    .ThenInclude(r => r.Educations)
                .Include(e => e.Resume)
                    .ThenInclude(r => r.WorkExperiences)
                .Include(e => e.Resume)
                    .ThenInclude(r => r.Skills)
                .Include(e => e.Resume)
                    .ThenInclude(r => r.Certifications)
                .Include(e => e.Resume)
                    .ThenInclude(r => r.Languages)
                .Include(e => e.Leaves)
                .Include(e => e.Expenses)
                .Include(e => e.Shifts)
                .Include(e => e.Notifications)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}