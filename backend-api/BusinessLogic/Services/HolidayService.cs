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
    public class HolidayService : IHolidayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HolidayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HolidayDto> GetHolidayByIdAsync(int holidayId)
        {
            var holiday = await _unitOfWork.Holidays.GetByIdAsync(holidayId);
            return _mapper.Map<HolidayDto>(holiday);
        }

        public async Task<IEnumerable<HolidayDto>> GetHolidaysByCompanyAsync(int companyId)
        {
            var holidays = await _unitOfWork.Holidays.GetHolidaysByCompanyAsync(companyId);
            return _mapper.Map<IEnumerable<HolidayDto>>(holidays);
        }

        public async Task<HolidayDto> CreateHolidayAsync(HolidayDto holidayDto)
        {
            var holiday = _mapper.Map<Holiday>(holidayDto);
            await _unitOfWork.Holidays.AddAsync(holiday);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<HolidayDto>(holiday);
        }

        public async Task<bool> UpdateHolidayAsync(HolidayDto holidayDto)
        {
            var holiday = await _unitOfWork.Holidays.GetByIdAsync(holidayDto.Id);
            if (holiday == null) return false;
            _mapper.Map(holidayDto, holiday);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteHolidayAsync(int holidayId)
        {
            var holiday = await _unitOfWork.Holidays.GetByIdAsync(holidayId);
            if (holiday == null) return false;
            await _unitOfWork.Holidays.DeleteAsync(holiday);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<HolidayDto>> GetUpcomingHolidaysAsync(int companyId, int daysAhead)
        {
            var endDate = DateTime.Now.AddDays(daysAhead);
            var holidays = await _unitOfWork.Holidays.GetHolidaysByCompanyAsync(companyId);
            var upcomingHolidays = holidays.Where(h => h.Date >= DateTime.Now && h.Date <= endDate);
            return _mapper.Map<IEnumerable<HolidayDto>>(upcomingHolidays);
        }
    }
}