using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DTOs
{
    public class ExceptionRecordResponseDTO
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string OSType { get; set; }
        public string OSVersion { get; set; }
        public string Message { get; set; }
        public string CallStack { get; set; }
        public DateTime ExceptionTime { get; set; }
    }
}
