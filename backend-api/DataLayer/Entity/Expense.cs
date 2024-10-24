using System;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Expense : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string ExpenseType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public string? ApproverComments { get; set; }
    }
}
