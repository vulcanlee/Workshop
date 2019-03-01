using LOBApp.DTOs;
using LOBApp.Helpers;
using LOBApp.Helpers.WebAPIs;
using LOBApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOBApp.Services
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
