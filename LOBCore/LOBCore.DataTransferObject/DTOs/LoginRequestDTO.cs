﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataTransferObject.DTOs
{
    public class LoginRequestDTO
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}
