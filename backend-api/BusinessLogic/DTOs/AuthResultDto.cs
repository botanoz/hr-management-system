using HrManagementSystem.BusinessLogic.DTOs;

public class AuthResultDto
{
    public bool Succeeded { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; }
    public string Error { get; set; }
}