using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBApp.Common.DTOs
{
    public class SystemEnvironmentResponseDTO
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public string AndroidVersion { get; set; }
        public string AndroidUrl { get; set; }
        public string iOSVersion { get; set; }
        public string iOSUrl { get; set; }
    }
}
