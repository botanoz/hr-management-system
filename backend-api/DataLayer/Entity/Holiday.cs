using System;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Holiday : BaseEntity
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Description { get; set; }
    }
}
