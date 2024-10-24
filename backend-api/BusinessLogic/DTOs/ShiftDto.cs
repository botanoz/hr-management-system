using System;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class ShiftDto
    {
        public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ShiftType { get; set; }
        public string Notes { get; set; }
    }
}