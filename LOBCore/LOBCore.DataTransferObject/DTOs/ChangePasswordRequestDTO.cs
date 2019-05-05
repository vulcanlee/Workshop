using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataTransferObject.DTOs
{
    public class ChangePasswordRequestDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
