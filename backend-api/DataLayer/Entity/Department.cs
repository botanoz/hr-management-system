using System.Collections.Generic;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
