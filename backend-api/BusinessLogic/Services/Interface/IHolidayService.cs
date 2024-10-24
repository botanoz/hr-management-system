using HrManagementSystem.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IHolidayService
    {
        Task<HolidayDto> GetHolidayByIdAsync(int holidayId);
        Task<IEnumerable<HolidayDto>> GetHolidaysByCompanyAsync(int companyId);
        Task<HolidayDto> CreateHolidayAsync(HolidayDto holidayDto);
        Task<bool> UpdateHolidayAsync(HolidayDto holidayDto);
        Task<bool> DeleteHolidayAsync(int holidayId);
        Task<IEnumerable<HolidayDto>> GetUpcomingHolidaysAsync(int companyId, int daysAhead);
    }
}