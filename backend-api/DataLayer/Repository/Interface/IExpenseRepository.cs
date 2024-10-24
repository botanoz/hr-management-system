using HrManagementSystem.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories.Interface
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetExpensesByEmployeeAsync(Guid employeeId);
        Task<decimal> GetTotalExpensesByEmployeeAsync(Guid employeeId, DateTime startDate, DateTime endDate);
    }
}
