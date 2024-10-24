using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        public ExpenseRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Expense>> GetExpensesByEmployeeAsync(Guid employeeId)
        {
            return await _context.Expenses
                .Where(e => e.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalExpensesByEmployeeAsync(Guid employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.Expenses
                .Where(e => e.EmployeeId == employeeId && e.ExpenseDate >= startDate && e.ExpenseDate <= endDate)
                .SumAsync(e => e.Amount);
        }
    }
}