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
            if (fooResult.Status != true)
            {
                await dialogService.DisplayAlertAsync("發生錯誤", fooResult.Message, "確定");
                return false;
            }

            systemStatusManager.SingleItem.UserID = loginManager.SingleItem.Id;
            systemStatusManager.SingleItem.Account = loginManager.SingleItem.Account;
            systemStatusManager.SingleItem.Department = loginManager.SingleItem.Department;
            systemStatusManager.SingleItem.IsLogin = true;
            systemStatusManager.SingleItem.LoginedTime = DateTime.Now;
            systemStatusManager.SingleItem.Token = loginManager.SingleItem.Token;
            systemStatusManager.SingleItem.RefreshToken = loginManager.SingleItem.RefreshToken;
            systemStatusManager.SingleItem.TokenExpireMinutes = loginManager.SingleItem.TokenExpireMinutes;
            systemStatusManager.SingleItem.RefreshTokenExpireDays = loginManager.SingleItem.RefreshTokenExpireDays;
            systemStatusManager.SingleItem.SetExpireDatetime();

            //await systemStatusManager.WriteToFileAsync();
            await AppStatusHelper.WriteAndUpdateAppStatus(systemStatusManager, appStatus);

            return true;
        }
    }
}
