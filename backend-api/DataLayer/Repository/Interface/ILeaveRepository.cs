using HrManagementSystem.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories.Interface
{
    public interface ILeaveRepository : IRepository<Leave>
    {
        Task<IEnumerable<Leave>> GetLeavesByEmployeeAsync(Guid employeeId);
    }
}
