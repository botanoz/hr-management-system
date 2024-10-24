using System;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
