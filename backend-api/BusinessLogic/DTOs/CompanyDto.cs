namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int EmployeeCount { get; set; }
        public bool IsApproved { get; set; }
        public DateTime SubscriptionEndDate { get; set; }
    }
    public class CompanySummaryDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int EmployeeCount { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
    }
}