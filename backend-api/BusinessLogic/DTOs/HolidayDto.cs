using System;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class HolidayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
    }
}