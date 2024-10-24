using System;
using System.Collections.Generic;

namespace HrManagementSystem.BusinessLogic.DTOs
{

    public class CompanyApprovalDto
    {
        public int CompanyId { get; set; }
        public bool IsApproved { get; set; }
        public string RejectionReason { get; set; }
    }

    public class SubscriptionUpdateDto
    {
        public int CompanyId { get; set; }
        public DateTime NewEndDate { get; set; }
    }

    public class UserRoleUpdateDto
    {
        public string UserId { get; set; }
        public string NewRole { get; set; }
    }

    public class SystemSettingsDto
    {
        public int DefaultSubscriptionDurationInMonths { get; set; }
        public decimal SubscriptionFee { get; set; }
        public int MaxEmployeesPerCompany { get; set; }
       
    }
    public class AdminDashboardDto : DashboardBaseDto
    {
        public int TotalCompanies { get; set; }
        public int PendingCompanyApprovals { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveSubscriptions { get; set; }
        public List<CompanySummaryDto> RecentlyRegisteredCompanies { get; set; }
        public List<CompanySummaryDto> UpcomingSubscriptionExpirations { get; set; }
    }
}