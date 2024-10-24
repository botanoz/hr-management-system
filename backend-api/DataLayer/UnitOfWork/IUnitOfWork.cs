using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.Repositories.Interface;
using System;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUsers { get; }
        ICompanyRepository Companies { get; }
        IDepartmentRepository Departments { get; }
        IEmployeeRepository Employees { get; }
        IEventRepository Events { get; }
        IExpenseRepository Expenses { get; }
        IHolidayRepository Holidays { get; }
        ILeaveRepository Leaves { get; }
        INotificationRepository Notifications { get; }
        IResumeRepository Resumes { get; }
        IShiftRepository Shifts { get; }
        IEducationRepository Educations { get; }
        IWorkExperienceRepository WorkExperiences { get; }
        ISkillRepository Skills { get; }
        ICertificationRepository Certifications { get; }
        ILanguageRepository Languages { get; }

        Task<int> CompleteAsync();
    }
}