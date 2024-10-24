using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class DashboardBaseDto
    {
        public List<NotificationDto> RecentNotifications { get; set; }
        public List<EventDto> UpcomingEvents { get; set; }
        public List<HolidayDto> UpcomingHolidays { get; set; }
    }
}
