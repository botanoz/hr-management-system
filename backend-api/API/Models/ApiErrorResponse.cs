using System.Collections.Generic;

namespace HrManagementSystem.API.Models
{
    public class ApiErrorResponse
    {
        public string Message { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }
    }
}