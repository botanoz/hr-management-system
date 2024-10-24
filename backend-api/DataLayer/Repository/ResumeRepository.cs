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
    public class ResumeRepository : Repository<Resume>, IResumeRepository
    {
        public ResumeRepository(AppDbContext context) : base(context) { }

        public async Task<Resume> GetResumeWithDetailsAsync(Guid employeeId)
        {
            return await _context.Resumes
                .Include(r => r.Educations)
                .Include(r => r.WorkExperiences)
                .Include(r => r.Skills)
                .Include(r => r.Certifications)
                .Include(r => r.Languages)
                .FirstOrDefaultAsync(r => r.EmployeeId == employeeId);
        }
    }

}
