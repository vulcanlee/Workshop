using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBApp.Common.DTOs
{
    public class NotificationTokenRequestDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public virtual UserDTO User { get; set; }
        public OSType OSType { get; set; }
        public DateTime RegistrationTime { get; set; }
        public bool Invalid { get; set; }
    }
}
