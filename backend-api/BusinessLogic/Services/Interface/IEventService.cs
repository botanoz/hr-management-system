using HrManagementSystem.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services.Interface
{
    public interface IEventService
    {
        Task<EventDto> GetEventByIdAsync(int eventId);
        Task<IEnumerable<EventDto>> GetEventsByCompanyAsync(int companyId);
        Task<EventDto> CreateEventAsync(EventDto eventDto);
        Task<bool> UpdateEventAsync(EventDto eventDto);
        Task<bool> DeleteEventAsync(int eventId);
        Task<IEnumerable<EventDto>> GetUpcomingEventsAsync(int companyId, int daysAhead);
    }
}