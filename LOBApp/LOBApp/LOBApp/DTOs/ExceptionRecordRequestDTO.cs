using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBApp.DTOs
{
    public class ExceptionRecordRequestDTO
    {
        public int Id { get; set; }
        public virtual UserDTO User { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string OSType { get; set; }
        public string OSVersion { get; set; }
        public string Message { get; set; }
        public string CallStack { get; set; }
        public DateTime ExceptionTime { get; set; }
    }
}
