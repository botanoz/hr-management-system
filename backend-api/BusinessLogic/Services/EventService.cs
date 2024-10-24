using AutoMapper;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventDto> GetEventByIdAsync(int eventId)
        {
            var @event = await _unitOfWork.Events.GetByIdAsync(eventId);
            return _mapper.Map<EventDto>(@event);
        }

        public async Task<IEnumerable<EventDto>> GetEventsByCompanyAsync(int companyId)
        {
            var events = await _unitOfWork.Events.GetEventsByCompanyAsync(companyId);
            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<EventDto> CreateEventAsync(EventDto eventDto)
        {
            var @event = _mapper.Map<Event>(eventDto);
            await _unitOfWork.Events.AddAsync(@event);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<EventDto>(@event);
        }

        public async Task<bool> UpdateEventAsync(EventDto eventDto)
        {
            var @event = await _unitOfWork.Events.GetByIdAsync(eventDto.Id);
            if (@event == null) return false;
            _mapper.Map(eventDto, @event);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var @event = await _unitOfWork.Events.GetByIdAsync(eventId);
            if (@event == null) return false;
            await _unitOfWork.Events.DeleteAsync(@event);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<EventDto>> GetUpcomingEventsAsync(int companyId, int daysAhead)
        {
            var endDate = DateTime.Now.AddDays(daysAhead);
            var events = await _unitOfWork.Events.GetEventsByCompanyAsync(companyId);
            var upcomingEvents = events.Where(e => e.EventDate >= DateTime.Now && e.EventDate <= endDate);
            return _mapper.Map<IEnumerable<EventDto>>(upcomingEvents);
        }
    }
}