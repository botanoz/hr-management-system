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
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Holiday>> GetHolidaysByCompanyAsync(int companyId)
        {
            return await _context.Holidays
                .Where(h => h.CompanyId == companyId)
                .ToListAsync();
        }
    }
}
