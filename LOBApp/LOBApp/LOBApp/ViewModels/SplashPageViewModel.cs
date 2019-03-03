using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;


namespace LOBApp.ViewModels
{
    using System.ComponentModel;
    using Acr.UserDialogs;
    using LOBApp.Common.DTOs;
    using LOBApp.Common.Helpers.ManagerHelps;
    using LOBApp.Common.Helpers.Utilities;
    using LOBApp.Models;
    using LOBApp.Common.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using LOBApp.Common.Models;

    public class SplashPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand CancellationCommand { get; set; }
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly SystemStatusManager systemStatusManager;
        private readonly SystemEnvironmentsManager systemEnvironmentsManager;
        private readonly RecordCacheHelper recordCacheHelper;
        private readonly AppStatus appStatus;
        private readonly ExceptionRecordsManager exceptionRecordsManager;

        public SplashPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            SystemStatusManager systemStatusManager, SystemEnvironmentsManager systemEnvironmentsManager,
            RecordCacheHelper recordCacheHelper, AppStatus appStatus,
            ExceptionRecordsManager exceptionRecordsManager)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.systemStatusManager = systemStatusManager;
            this.systemEnvironmentsManager = systemEnvironmentsManager;
            this.recordCacheHelper = recordCacheHelper;
            this.appStatus = appStatus;
            this.exceptionRecordsManager = exceptionRecordsManager;
            CancellationCommand = new DelegateCommand(() =>
            {

            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await AppStatusHelper.ReadAndUpdateAppStatus(systemStatusManager, appStatus);
            if (appStatus.SystemStatus.IsLogin == false)
            {
                await navigationService.NavigateAsync("/LoginPage");
                return;
            }

            #region 使用者已經成功登入了，接下來要更新相關資料
            if (UtilityHelper.IsConnected() == false)
            {
                await dialogService.DisplayAlertAsync("警告", "無網路連線可用，請檢查網路狀態", "確定");
                return;
            }

            using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
            {
                await recordCacheHelper.RefreshAsync(fooIProgressDialog);

                fooIProgressDialog.Title = "請稍後，上傳例外異常";
                await exceptionRecordsManager.ReadFromFileAsync();
                if (exceptionRecordsManager.Items.Count > 0)
                {
                    var fooExceptions = exceptionRecordsManager.Items;
                    var fooExceptionRecordRequestDTOs = new List<ExceptionRecordRequestDTO>();

                    foreach (var item in fooExceptions)
                    {
                        var fooExceptionRecordRequestDTO = new ExceptionRecordRequestDTO()
                        {
                            CallStack = item.CallStack,
                            DeviceModel = item.DeviceModel,
                            DeviceName = item.DeviceName,
                            ExceptionTime = item.ExceptionTime,
                            Message = item.Message,
                            OSType = item.OSType,
                            OSVersion = item.OSVersion,
                            User = new UserDTO() { Id = appStatus.SystemStatus.UserID },
                        };
                        fooExceptionRecordRequestDTOs.Add(fooExceptionRecordRequestDTO);
                    }
                    var fooResult = await exceptionRecordsManager.PostAsync(fooExceptionRecordRequestDTOs);
                    if(fooResult.Status == true)
                    {
                        exceptionRecordsManager.Items.Clear();
                        await exceptionRecordsManager.WriteToFileAsync();
                    }
                }
            }
            await navigationService.NavigateAsync("/MDPage/NaviPage/HomePage");
            #endregion
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

    }
}
