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
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Language>> GetLanguagesByResumeAsync(int resumeId)
        {
            return await _context.Languages
                .Where(l => l.ResumeId == resumeId)
                .ToListAsync();
        }
    }
}
