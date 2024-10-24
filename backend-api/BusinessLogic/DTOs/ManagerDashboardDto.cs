using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class ManagerDashboardDto : DashboardBaseDto
    {
        public int TotalEmployees { get; set; }
        public int PendingLeaveRequests { get; set; }
        public int PendingExpenseRequests { get; set; }
        public decimal TotalExpensesThisMonth { get; set; }
        public List<EmployeeDto> UpcomingBirthdays { get; set; }
        public int ActiveShifts { get; set; }
        public List<DepartmentSummaryDto> DepartmentSummaries { get; set; }
    }
}
