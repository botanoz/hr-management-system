using HrManagementSystem.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.Repositories.Interface
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsByEmployeeAsync(Guid employeeId);
        Task<IEnumerable<Notification>> GetNotificationsBySenderAsync(Guid senderId);
    }
}
