namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class ResumeDto
    {
        public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<EducationDto> Educations { get; set; }
        public List<WorkExperienceDto> WorkExperiences { get; set; }
        public List<SkillDto> Skills { get; set; }
        public List<CertificationDto> Certifications { get; set; }
        public List<LanguageDto> Languages { get; set; }
        public string AdditionalInformation { get; set; }
    }
    public class CertificationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Issuer { get; set; }
        public DateTime DateIssued { get; set; }
    }
    public class EducationDto
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FieldOfStudy { get; set; }
    }
    public class LanguageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Proficiency { get; set; }
    }
    public class SkillDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Proficiency { get; set; }
    }
    public class WorkExperienceDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}