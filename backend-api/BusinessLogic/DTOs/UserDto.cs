namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Position { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsManager { get; set; }
        public string Role { get; set; }
    }
}