using System;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Shift : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ShiftType { get; set; } // Vardiya türü (gece, gündüz vb.)
        public string Notes { get; set; } // Vardiya ile ilgili notlar
    }
}
