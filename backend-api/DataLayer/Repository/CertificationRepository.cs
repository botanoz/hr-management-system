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
    public class CertificationRepository : Repository<Certification>, ICertificationRepository
    {
        public CertificationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Certification>> GetCertificationsByResumeAsync(int resumeId)
        {
            return await _context.Certifications
                .Where(c => c.ResumeId == resumeId)
                .ToListAsync();
        }
    }
}
