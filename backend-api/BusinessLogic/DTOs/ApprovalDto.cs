using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrManagementSystem.BusinessLogic.DTOs
{
    public class ApprovalDto
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}
