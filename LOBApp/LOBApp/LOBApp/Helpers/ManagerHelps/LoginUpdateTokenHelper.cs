using LOBApp.DTOs;
using LOBApp.Models;
using LOBApp.Services;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LOBApp.Helpers.ManagerHelps
{
    public class LoginUpdateTokenHelper
    {
        public static async Task<bool> UserLoginAsync(IPageDialogService dialogService,
            LoginManager loginManager, SystemStatusManager systemStatusManager, LoginRequestDTO loginRequestDTO,
            AppStatus appStatus)
        {
            var fooResult = await loginManager.PostAsync(loginRequestDTO);
            if (fooResult.Status != APIResultStatus.Success)
            {
                await dialogService.DisplayAlertAsync("發生錯誤", fooResult.Message, "確定");
                return false;
            }

            systemStatusManager.Items.IsLogin = true;
            systemStatusManager.Items.LoginedTime = DateTime.Now;
            systemStatusManager.Items.Token = loginManager.Items.Token;
            systemStatusManager.Items.RefreshToken = loginManager.Items.RefreshToken;
            systemStatusManager.Items.TokenExpireMinutes = loginManager.Items.TokenExpireMinutes;
            systemStatusManager.Items.RefreshTokenExpireDays = loginManager.Items.RefreshTokenExpireDays;
            systemStatusManager.Items.SetExpireDatetime();

            //await systemStatusManager.WriteToFileAsync();
            await AppStatusHelper.WriteAndUpdateAppStatus(systemStatusManager, appStatus);

            return true;
        }
    }
}
