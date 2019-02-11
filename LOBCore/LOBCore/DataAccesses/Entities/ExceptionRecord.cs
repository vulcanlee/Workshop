using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataAccesses.Entities
{
    public enum OSType
    {
        iOS,
        Android
    }
    public class ExceptionRecord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual LobUser User { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string OSType { get; set; }
        public string OSVersion { get; set; }
        public string Message { get; set; }
        public string CallStack { get; set; }
        public DateTime ExceptionTime { get; set; }
    }
}
