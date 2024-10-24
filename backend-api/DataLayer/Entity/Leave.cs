using System;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Leave : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } // "Pending", "Approved", "Rejected"
        public string Comments { get; set; }
        public Guid? ApproverId { get; set; }
        public Employee Approver { get; set; }
    }
}
