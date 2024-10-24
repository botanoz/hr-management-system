using AutoMapper;
using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.BusinessLogic.DTOs;
using System;
using System.Linq;

namespace HrManagementSystem.BusinessLogic.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ReverseMap();

            CreateMap<Company, CompanyDto>().ReverseMap();

            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ReverseMap();

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ReverseMap();

            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ReverseMap();

            CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ReverseMap();

            CreateMap<Holiday, HolidayDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ReverseMap();

            CreateMap<Leave, LeaveDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ForMember(dest => dest.ApproverName, opt => opt.MapFrom(src => src.Approver != null ? $"{src.Approver.FirstName} {src.Approver.LastName}" : null))
                .ReverseMap();

            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? $"{src.Employee.FirstName} {src.Employee.LastName}" : null))
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ReverseMap();

            CreateMap<Resume, ResumeDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ReverseMap();

            CreateMap<Shift, ShiftDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ReverseMap();

            CreateMap<Education, EducationDto>().ReverseMap();
            CreateMap<WorkExperience, WorkExperienceDto>().ReverseMap();
            CreateMap<Skill, SkillDto>().ReverseMap();
            CreateMap<Certification, CertificationDto>().ReverseMap();
            CreateMap<Language, LanguageDto>().ReverseMap();

            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<Company, CompanySummaryDto>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.EmployeeCount));

            CreateMap<Department, DepartmentSummaryDto>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees.Count))
                .ForMember(dest => dest.PendingLeaveRequests, opt => opt.MapFrom(src => src.Employees.SelectMany(e => e.Leaves).Count(l => l.Status == "Pending")))
                .ForMember(dest => dest.TotalExpensesThisMonth, opt => opt.MapFrom(src =>
                    src.Employees.SelectMany(e => e.Expenses)
                        .Where(ex => ex.ExpenseDate.Month == DateTime.Now.Month && ex.ExpenseDate.Year == DateTime.Now.Year)
                        .Sum(ex => ex.Amount)));

            // Dashboard DTOs
            CreateMap<Employee, EmployeeDashboardDto>()
                .ForMember(dest => dest.PendingLeaveRequests, opt => opt.MapFrom(src => src.Leaves.Count(l => l.Status == "Pending")))
                .ForMember(dest => dest.PendingExpenseRequests, opt => opt.MapFrom(src => src.Expenses.Count(e => e.Status == "Pending")))
                .ForMember(dest => dest.UpcomingShifts, opt => opt.MapFrom(src => src.Shifts.Where(s => s.StartTime > DateTime.Now).OrderBy(s => s.StartTime).Take(5)))
                .ForMember(dest => dest.RecentNotifications, opt => opt.MapFrom(src => src.Notifications.OrderByDescending(n => n.DateSent).Take(5)))
                .ForMember(dest => dest.UpcomingEvents, opt => opt.Ignore())
                .ForMember(dest => dest.UpcomingHolidays, opt => opt.Ignore())
                .ForMember(dest => dest.RemainingLeaveDays, opt => opt.Ignore())
                .ForMember(dest => dest.TotalExpensesThisMonth, opt => opt.MapFrom(src =>
                    src.Expenses.Where(e => e.ExpenseDate.Month == DateTime.Now.Month && e.ExpenseDate.Year == DateTime.Now.Year).Sum(e => e.Amount)));

            CreateMap<Company, ManagerDashboardDto>()
                .ForMember(dest => dest.TotalEmployees, opt => opt.MapFrom(src => src.EmployeeCount))
                .ForMember(dest => dest.PendingLeaveRequests, opt => opt.MapFrom(src => src.Employees.SelectMany(e => e.Leaves).Count(l => l.Status == "Pending")))
                .ForMember(dest => dest.PendingExpenseRequests, opt => opt.MapFrom(src => src.Employees.SelectMany(e => e.Expenses).Count(ex => ex.Status == "Pending")))
                .ForMember(dest => dest.TotalExpensesThisMonth, opt => opt.MapFrom(src =>
                    src.Employees.SelectMany(e => e.Expenses)
                        .Where(ex => ex.ExpenseDate.Month == DateTime.Now.Month && ex.ExpenseDate.Year == DateTime.Now.Year)
                        .Sum(ex => ex.Amount)))
                .ForMember(dest => dest.UpcomingEvents, opt => opt.Ignore())
                .ForMember(dest => dest.UpcomingBirthdays, opt => opt.MapFrom(src => src.Employees.Where(e => e.Birthdate.Month == DateTime.Now.Month && e.Birthdate.Day >= DateTime.Now.Day).OrderBy(e => e.Birthdate.Day).Take(5)))
                .ForMember(dest => dest.ActiveShifts, opt => opt.MapFrom(src => src.Employees.SelectMany(e => e.Shifts).Count(s => s.StartTime <= DateTime.Now && s.EndTime >= DateTime.Now)))
                .ForMember(dest => dest.DepartmentSummaries, opt => opt.MapFrom(src => src.Departments))
                .ForMember(dest => dest.RecentNotifications, opt => opt.Ignore())
                .ForMember(dest => dest.UpcomingHolidays, opt => opt.Ignore());

            // AdminDashboardDto doesn't have a direct entity mapping, it will be handled in the service layer

            // Additional mappings for updated entities
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Assuming Id is now a string (Guid)
                .ReverseMap();

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId)) // Mapping for Guid EmployeeId
                .ReverseMap();

            CreateMap<Expense, ExpenseDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId)) // Mapping for Guid EmployeeId
                .ReverseMap();

            CreateMap<Leave, LeaveDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.ApproverId, opt => opt.MapFrom(src => src.ApproverId))
                .ReverseMap();

            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
                .ReverseMap();

            CreateMap<Resume, ResumeDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ReverseMap();

            CreateMap<Shift, ShiftDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ReverseMap();
        }
    }
}