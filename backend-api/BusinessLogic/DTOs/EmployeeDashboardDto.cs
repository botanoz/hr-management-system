using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class EmployeeDashboardDto : DashboardBaseDto
    {
        public int PendingLeaveRequests { get; set; }
        public int PendingExpenseRequests { get; set; }
        public List<ShiftDto> UpcomingShifts { get; set; }
        public int RemainingLeaveDays { get; set; }
        public decimal TotalExpensesThisMonth { get; set; }
    }
}
