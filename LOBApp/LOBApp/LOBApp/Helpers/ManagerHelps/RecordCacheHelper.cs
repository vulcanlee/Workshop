using Acr.UserDialogs;
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
    public class RecordCacheHelper
    {
        private readonly IPageDialogService dialogService;
        private readonly DepartmentsManager departmentsManager;
        private readonly SystemEnvironmentsManager systemEnvironmentsManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;
        private readonly RefreshTokenManager refreshTokenManager;
        private readonly LeaveFormTypesManager leaveFormTypesManager;

        public RecordCacheHelper(IPageDialogService dialogService, DepartmentsManager departmentsManager,
            SystemEnvironmentsManager systemEnvironmentsManager,
            SystemStatusManager systemStatusManager, AppStatus appStatus, RefreshTokenManager refreshTokenManager,
            LeaveFormTypesManager leaveFormTypesManager)
        {
            this.dialogService = dialogService;
            this.departmentsManager = departmentsManager;
            this.systemEnvironmentsManager = systemEnvironmentsManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            this.refreshTokenManager = refreshTokenManager;
            this.leaveFormTypesManager = leaveFormTypesManager;
        }

        public async Task<bool> RefreshAsync(IProgressDialog progressDialog)
        {
            progressDialog.Title = $"檢查與更新存取權杖";
            bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
            if(fooRefreshTokenResult == false)
            {
                return false;
            }
            progressDialog.Title = $"更新系統最新狀態資料中";
            APIResult fooAPIResult = await systemEnvironmentsManager.GetAsync();
            if (fooAPIResult.Status != APIResultStatus.Success)
            {
                await dialogService.DisplayAlertAsync("發生錯誤", fooAPIResult.Message, "確定");
                return false;
            }
            progressDialog.Title = $"更新請假類別代碼資料中";
            fooAPIResult = await leaveFormTypesManager.GetAsync();
            if (fooAPIResult.Status != APIResultStatus.Success)
            {
                await dialogService.DisplayAlertAsync("發生錯誤", fooAPIResult.Message, "確定");
                return false;
            }
            progressDialog.Title = $"更新部門資料中";
             fooAPIResult = await departmentsManager.GetAsync();
            if (fooAPIResult.Status != APIResultStatus.Success)
            {
                await dialogService.DisplayAlertAsync("發生錯誤", fooAPIResult.Message, "確定");
                return false;
            }

            return true;
        }
    }
}
