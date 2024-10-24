using HrManagementSystem.DataLayer.Entities;
using HrManagementSystem.DataLayer.Repositories;
using HrManagementSystem.DataLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace HrManagementSystem.DataLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ApplicationUsers = new ApplicationUserRepository(_context);
            Companies = new CompanyRepository(_context);
            Departments = new DepartmentRepository(_context);
            Employees = new EmployeeRepository(_context);
            Events = new EventRepository(_context);
            Expenses = new ExpenseRepository(_context);
            Holidays = new HolidayRepository(_context);
            Leaves = new LeaveRepository(_context);
            Notifications = new NotificationRepository(_context);
            Resumes = new ResumeRepository(_context);
            Shifts = new ShiftRepository(_context);
            Educations = new EducationRepository(_context);
            WorkExperiences = new WorkExperienceRepository(_context);
            Skills = new SkillRepository(_context);
            Certifications = new CertificationRepository(_context);
            Languages = new LanguageRepository(_context);
        }

        public IApplicationUserRepository ApplicationUsers { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public IDepartmentRepository Departments { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IEventRepository Events { get; private set; }
        public IExpenseRepository Expenses { get; private set; }
        public IHolidayRepository Holidays { get; private set; }
        public ILeaveRepository Leaves { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IResumeRepository Resumes { get; private set; }
        public IShiftRepository Shifts { get; private set; }
        public IEducationRepository Educations { get; private set; }
        public IWorkExperienceRepository WorkExperiences { get; private set; }
        public ISkillRepository Skills { get; private set; }
        public ICertificationRepository Certifications { get; private set; }
        public ILanguageRepository Languages { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}