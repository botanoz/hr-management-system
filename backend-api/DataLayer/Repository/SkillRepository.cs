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
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        public SkillRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Skill>> GetSkillsByResumeAsync(int resumeId)
        {
            return await _context.Skills
                .Where(s => s.ResumeId == resumeId)
                .ToListAsync();
        }
    }
}
