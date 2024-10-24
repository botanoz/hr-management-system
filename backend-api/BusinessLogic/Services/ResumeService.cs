using System.Threading.Tasks;
using AutoMapper;
using HrManagementSystem.BusinessLogic.DTOs;
using HrManagementSystem.BusinessLogic.Services.Interface;
using HrManagementSystem.DataLayer.UnitOfWork;
using HrManagementSystem.DataLayer.Entities;

namespace HrManagementSystem.BusinessLogic.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ResumeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResumeDto> GetResumeByIdAsync(int id)
        {
            var resume = await _unitOfWork.Resumes.GetByIdAsync(id);
            return _mapper.Map<ResumeDto>(resume);
        }

        public async Task<ResumeDto> GetResumeByEmployeeIdAsync(Guid employeeId)
        {
            var resume = await _unitOfWork.Resumes.GetResumeWithDetailsAsync(employeeId);
            return _mapper.Map<ResumeDto>(resume);
        }

        public async Task<ResumeDto> CreateResumeAsync(ResumeDto resumeDto)
        {
            var resume = _mapper.Map<Resume>(resumeDto);
            await _unitOfWork.Resumes.AddAsync(resume);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ResumeDto>(resume);
        }

        public async Task UpdateResumeAsync(ResumeDto resumeDto)
        {
            var existingResume = await _unitOfWork.Resumes.GetByIdAsync(resumeDto.Id);
            if (existingResume != null)
            {
                _mapper.Map(resumeDto, existingResume);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeleteResumeAsync(int id)
        {
            var resume = await _unitOfWork.Resumes.GetByIdAsync(id);
            if (resume != null)
            {
                await _unitOfWork.Resumes.DeleteAsync(resume);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}