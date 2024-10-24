using System;
using System.Collections.Generic;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Employee : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public Resume Resume { get; set; }
        public ICollection<Leave> Leaves { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Shift> Shifts { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
