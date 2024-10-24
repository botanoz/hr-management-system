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
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        public LeaveRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Leave>> GetLeavesByEmployeeAsync(Guid employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}
