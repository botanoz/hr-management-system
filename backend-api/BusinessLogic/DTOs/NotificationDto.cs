using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime? DateRead { get; set; }
        public bool IsRead => DateRead.HasValue;
        public Guid? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; }
        public string NotificationType { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Priority { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

}
