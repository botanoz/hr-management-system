using HrManagementSystem.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface INotificationService
    {
        Task<NotificationDto> GetNotificationByIdAsync(int notificationId);
        Task<IEnumerable<NotificationDto>> GetNotificationsByEmployeeAsync(Guid employeeId);
        Task<NotificationDto> CreateNotificationAsync(NotificationDto notificationDto);
        Task<bool> UpdateNotificationAsync(NotificationDto notificationDto);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<bool> MarkNotificationAsReadAsync(int notificationId);
        Task<int> GetUnreadNotificationCountAsync(Guid employeeId);
    }
}
