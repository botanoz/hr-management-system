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

    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Notification>> GetNotificationsByEmployeeAsync(Guid employeeId)
        {
            return await _context.Notifications
                .Where(n => n.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsBySenderAsync(Guid senderId)
        {
            return await _context.Notifications
                .Where(n => n.SenderId == senderId)
                .ToListAsync();
        }
    }
}
