using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IReportService
    {
        Task<byte[]> GenerateEmployeeReportAsync(int companyId);
        Task<byte[]> GenerateExpenseReportAsync(int companyId, DateTime startDate, DateTime endDate);
        Task<byte[]> GenerateLeaveReportAsync(int companyId, DateTime startDate, DateTime endDate);
        Task<byte[]> GeneratePayrollReportAsync(int companyId, DateTime startDate, DateTime endDate);
    }
}
