using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.UnitOfWork;
using HrManagementSystem.DataLayer.Entities;

namespace HrManagementSystem.BusinessLogic.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NotificationDto> GetNotificationByIdAsync(int notificationId)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByEmployeeAsync(Guid employeeId)
        {
            var notifications = await _unitOfWork.Notifications.GetNotificationsByEmployeeAsync(employeeId);
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        public async Task<NotificationDto> CreateNotificationAsync(NotificationDto notificationDto)
        {
            var notification = _mapper.Map<Notification>(notificationDto);
            notification.DateSent = DateTime.Now;
            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task<bool> UpdateNotificationAsync(NotificationDto notificationDto)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationDto.Id);
            if (notification == null) return false;
            _mapper.Map(notificationDto, notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
            if (notification == null) return false;
            await _unitOfWork.Notifications.DeleteAsync(notification);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
            if (notification == null) return false;
            notification.DateRead = DateTime.Now;
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<int> GetUnreadNotificationCountAsync(Guid employeeId)
        {
            var notifications = await _unitOfWork.Notifications.GetNotificationsByEmployeeAsync(employeeId);
            return notifications.Count(n => !n.IsRead);
        }
    }
}