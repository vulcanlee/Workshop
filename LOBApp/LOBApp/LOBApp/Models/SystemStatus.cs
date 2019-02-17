using System;
using System.Collections.Generic;
using System.Text;

namespace LOBApp.Models
{
    public class SystemStatus
    {
        public bool IsLogin { get; set; }
        public DateTime LoginedTime { get; set; }
        public string Token { get; set; }
        public int TokenExpireMinutes { get; set; }
        public DateTime TokenExpireDatetime { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpireDays { get; set; }
        public DateTime RefreshTokenExpireDatetime { get; set; }

        public void SetExpireDatetime()
        {
            TokenExpireDatetime = DateTime.UtcNow.AddMinutes(TokenExpireMinutes);
            RefreshTokenExpireDatetime = DateTime.UtcNow.AddDays(RefreshTokenExpireDays);
        }
    }
}
