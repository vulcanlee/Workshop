using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBApp.DTOs
{
    public class SuggestionRequestDTO
    {
        public virtual UserDTO User { get; set; }
        public DateTime SubmitTime { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
