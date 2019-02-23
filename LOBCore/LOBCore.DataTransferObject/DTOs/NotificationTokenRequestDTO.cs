using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataTransferObject.DTOs
{
    public class NotificationTokenRequestDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public virtual UserDTO User { get; set; }
        public OSTypeDTO OSType { get; set; }
        public DateTime RegistrationTime { get; set; }
        public bool Invalid { get; set; }
    }
}
