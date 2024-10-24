using System;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class LeaveDto
    {
        public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public Guid? ApproverId { get; set; }
        public string ApproverName { get; set; }
    }
}