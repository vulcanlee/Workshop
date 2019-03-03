using LOBApp.Common.DTOs;
using LOBApp.Common.Helpers;
using LOBApp.Common.Helpers.WebAPIs;
using LOBApp.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOBApp.Common.Services
{
    public class SystemStatusManager : BaseWebAPI<SystemStatus>
    {
        public SystemStatusManager()
            : base()
        {
            isCollection = false;
        }
    }
}
