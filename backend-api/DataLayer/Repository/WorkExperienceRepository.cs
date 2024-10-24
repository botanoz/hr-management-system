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
    public class WorkExperienceRepository : Repository<WorkExperience>, IWorkExperienceRepository
    {
        public WorkExperienceRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<WorkExperience>> GetWorkExperiencesByResumeAsync(int resumeId)
        {
            return await _context.WorkExperiences
                .Where(w => w.ResumeId == resumeId)
                .ToListAsync();
        }
    }
}
