namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int EmployeeCount { get; set; }
    }
    public class DepartmentSummaryDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public int PendingLeaveRequests { get; set; }
        public decimal TotalExpensesThisMonth { get; set; }
    }
}