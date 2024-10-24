using HrManagementSystem.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories.Interface
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetEmployeesByCompanyAsync(int companyId);
        Task<IEnumerable<Employee>> GetUpcomingBirthdaysAsync(int companyId, int daysAhead);
        Task<Employee> GetEmployeeWithFullDetailsAsync(Guid employeeId);
        Task<Employee> GetEmployeeByEmailAsync(string email);
    }
}
