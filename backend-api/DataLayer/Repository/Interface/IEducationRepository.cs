﻿using HrManagementSystem.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories.Interface
{
    public interface IEducationRepository : IRepository<Education>
    {
        Task<IEnumerable<Education>> GetEducationsByResumeAsync(int resumeId);
    }
}