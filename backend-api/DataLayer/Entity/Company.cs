using System;
using System.Collections.Generic;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int EmployeeCount { get; set; }
        public bool IsApproved { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Holiday> Holidays { get; set; }
    }
}
