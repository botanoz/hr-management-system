using System.Collections.Generic;

namespace HrManagementSystem.DataLayer.Entities
{
    public class Resume : BaseEntity
    {
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public ICollection<Certification> Certifications { get; set; } = new List<Certification>();
        public ICollection<Language> Languages { get; set; } = new List<Language>();
        public string AdditionalInformation { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }

    public class Education
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FieldOfStudy { get; set; }
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }

    public class WorkExperience
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }

    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Proficiency { get; set; } // Örneğin: Beginner, Intermediate, Expert
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }

    public class Certification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Issuer { get; set; }
        public DateTime DateIssued { get; set; }
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }

    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Proficiency { get; set; } // Örneğin: Basic, Conversational, Fluent
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }
}
