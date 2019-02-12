﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DTOs
{
    public class LoginResponseDTO
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public virtual DepartmentDTO Department { get; set; }
    }
}