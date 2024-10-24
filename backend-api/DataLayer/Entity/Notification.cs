using System;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime? DateRead { get; set; }
        public bool IsRead => DateRead.HasValue;
        public Guid? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid SenderId { get; set; }
        public Employee Sender { get; set; }
        public string NotificationType { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Priority { get; set; } // e.g., "High", "Medium", "Low"
        public DateTime? ExpiryDate { get; set; }
    }
}
