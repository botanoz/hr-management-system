using System;
using System.Collections.Generic;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class EmployeeReportDto
    {
        public int TotalEmployees { get; set; }
        public Dictionary<string, int> EmployeesByDepartment { get; set; }
        public Dictionary<string, int> EmployeesByPosition { get; set; }
        public Dictionary<int, int> EmployeesByYearsOfService { get; set; }
    }

    public class LeaveReportDto
    {
        public int TotalLeaveRequests { get; set; }
        public int ApprovedLeaves { get; set; }
        public int PendingLeaves { get; set; }
        public int RejectedLeaves { get; set; }
        public Dictionary<string, int> LeavesByType { get; set; }
        public Dictionary<string, int> LeavesByDepartment { get; set; }
    }

    public class ExpenseReportDto
    {
        public decimal TotalExpenses { get; set; }
        public decimal ApprovedExpenses { get; set; }
        public decimal PendingExpenses { get; set; }
        public decimal RejectedExpenses { get; set; }
        public Dictionary<string, decimal> ExpensesByType { get; set; }
        public Dictionary<string, decimal> ExpensesByDepartment { get; set; }
    }

    public class ShiftReportDto
    {
        public int TotalShifts { get; set; }
        public Dictionary<string, int> ShiftsByType { get; set; }
        public Dictionary<string, int> ShiftsByDepartment { get; set; }
        public int OverTimeHours { get; set; }
    }

    public class CompanyReportDto
    {
        public EmployeeReportDto EmployeeReport { get; set; }
        public LeaveReportDto LeaveReport { get; set; }
        public ExpenseReportDto ExpenseReport { get; set; }
        public ShiftReportDto ShiftReport { get; set; }
    }
}