using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class EmployeeSummaryDto
    {
        public EmployeeDto Employee { get; set; }
        public int TotalLeavesTaken { get; set; }
        public decimal TotalExpensesSubmitted { get; set; }
    }

    public class CompanyOverviewDto
    {
        public CompanyDto Company { get; set; }
        public int TotalDepartments { get; set; }
        public List<DepartmentSummaryDto> Departments { get; set; }
    }

    public class PendingApprovalsDto
    {
        public List<LeaveDto> PendingLeaves { get; set; }
        public List<ExpenseDto> PendingExpenses { get; set; }
    }

}
