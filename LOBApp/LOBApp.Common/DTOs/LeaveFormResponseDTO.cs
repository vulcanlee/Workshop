using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBApp.Common.DTOs
{
    public class LeaveFormResponseDTO
    {
        public int Id { get; set; }
        public UserDTO user { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalHours { get; set; }
        public LeaveFormTypeDTO leaveFormType { get; set; }
        public string Description { get; set; }
    }
}
