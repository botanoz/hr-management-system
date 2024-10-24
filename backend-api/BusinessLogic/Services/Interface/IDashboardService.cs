using HrManagementSystem.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IDashboardService
    {
        Task<EmployeeDashboardDto> GetEmployeeDashboardAsync(string email);
        Task<ManagerDashboardDto> GetManagerDashboardAsync(string email);
        Task<AdminDashboardDto> GetAdminDashboardAsync();
    }
}
