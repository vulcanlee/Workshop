using LOBApp.Models;
using LOBApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LOBApp.Helpers.ManagerHelps
{
    public class AppStatusHelper
    {
        public static async Task<bool> ReadAndUpdateAppStatus(SystemStatusManager systemStatusManager, AppStatus appStatus)
        {
            await systemStatusManager.ReadFromFileAsync();
            appStatus.SystemStatus = systemStatusManager.Items;
            return true;
        }
        public static async Task<bool> WriteAndUpdateAppStatus(SystemStatusManager systemStatusManager, AppStatus appStatus)
        {
            await systemStatusManager.WriteToFileAsync();
            appStatus.SystemStatus = systemStatusManager.Items;
            return true;
        }
    }
}
