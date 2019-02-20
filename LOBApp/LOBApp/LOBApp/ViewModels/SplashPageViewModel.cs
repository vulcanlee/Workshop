using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;


namespace LOBApp.ViewModels
{
    using System.ComponentModel;
    using Acr.UserDialogs;
    using LOBApp.Helpers.ManagerHelps;
    using LOBApp.Helpers.Utilities;
    using LOBApp.Models;
    using LOBApp.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
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

        public SplashPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            SystemStatusManager systemStatusManager, SystemEnvironmentsManager systemEnvironmentsManager,
            RecordCacheHelper recordCacheHelper, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.systemStatusManager = systemStatusManager;
            this.systemEnvironmentsManager = systemEnvironmentsManager;
            this.recordCacheHelper = recordCacheHelper;
            this.appStatus = appStatus;
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
                return ;
            }

            using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
            {
                await recordCacheHelper.RefreshAsync(fooIProgressDialog);
            }
            #endregion
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

    }
}
