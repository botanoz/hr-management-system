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
    public class ShiftRepository : Repository<Shift>, IShiftRepository
    {
        public ShiftRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Shift>> GetShiftsByEmployeeAsync(Guid employeeId)
        {
            return await _context.Shifts
                .Where(s => s.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}
