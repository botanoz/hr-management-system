using System;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}