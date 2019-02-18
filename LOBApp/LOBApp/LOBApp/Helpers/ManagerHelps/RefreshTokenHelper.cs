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
    public class RefreshTokenHelper
    {
        public static async Task<bool> CheckAndRefreshToken(IPageDialogService dialogService,
            RefreshTokenManager refreshTokenManager, SystemStatusManager systemStatusManager, 
            AppStatus appStatus)
        {
            if(appStatus.SystemStatus.TokenExpireDatetime > DateTime.Now)
            {
                #region Token 尚在有效期限
                return true;
                #endregion
            }
            else
            {
                #region Token 已經失效了，需要更新
                var fooResult = await refreshTokenManager.GetAsync();
                if (fooResult.Status != APIResultStatus.Success)
                {
                    await dialogService.DisplayAlertAsync("發生錯誤", fooResult.Message, "確定");
                    return false;
                }
                systemStatusManager.Items = appStatus.SystemStatus;
                systemStatusManager.Items.IsLogin = true;
                systemStatusManager.Items.LoginedTime = DateTime.Now;
                systemStatusManager.Items.Token = refreshTokenManager.Items.Token;
                systemStatusManager.Items.RefreshToken = refreshTokenManager.Items.RefreshToken;
                systemStatusManager.Items.TokenExpireMinutes = refreshTokenManager.Items.TokenExpireMinutes;
                systemStatusManager.Items.RefreshTokenExpireDays = refreshTokenManager.Items.RefreshTokenExpireDays;
                systemStatusManager.Items.SetExpireDatetime();
                #endregion
            }

            //await systemStatusManager.WriteToFileAsync();
            await AppStatusHelper.WriteAndUpdateAppStatus(systemStatusManager, appStatus);

            return true;
        }
    }
}
