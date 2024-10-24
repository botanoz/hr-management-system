using System;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string ExpenseType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ApproverComments { get; set; }
    }
}