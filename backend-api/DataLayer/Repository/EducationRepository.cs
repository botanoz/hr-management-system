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
    public class EducationRepository : Repository<Education>, IEducationRepository
    {
        public EducationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Education>> GetEducationsByResumeAsync(int resumeId)
        {
            return await _context.Educations
                .Where(e => e.ResumeId == resumeId)
                .ToListAsync();
        }
    }
}
