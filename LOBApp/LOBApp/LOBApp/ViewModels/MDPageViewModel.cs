using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LOBApp.ViewModels
{
    using System.ComponentModel;
    using Acr.UserDialogs;
    using LOBApp.Common.Helpers.ManagerHelps;
    using LOBApp.Models;
    using LOBApp.Common.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using LOBApp.Common.Models;

    public class MDPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly LoginManager loginManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;
        private readonly LogoutCleanHelper logoutCleanHelper;

        public DelegateCommand HomeCommand { get; set; }
        public DelegateCommand UserGroupCommand { get; set; }
        public DelegateCommand LeaveFormCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }
        public DelegateCommand SuggestionCommand { get; set; }
        public DelegateCommand ExceptionCommand { get; set; }
        public MDPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
             LoginManager loginManager, SystemStatusManager systemStatusManager,
             AppStatus appStatus, LogoutCleanHelper logoutCleanHelper)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.loginManager = loginManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            this.logoutCleanHelper = logoutCleanHelper;
            HomeCommand = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("/MDPage/NaviPage/HomePage");
            });
            SuggestionCommand = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("/MDPage/NaviPage/SuggestionPage");
            });
            ExceptionCommand = new DelegateCommand(() =>
            {
                throw new Exception("發生了一個自訂的例外異常");
            });
            UserGroupCommand = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("/MDPage/NaviPage/CommUsePage");
            });
            LeaveFormCommand = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("/MDPage/NaviPage/LeaveFormPage");
            });
            LogoutCommand = new DelegateCommand(async () =>
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
                {
                    await logoutCleanHelper.LogoutCleanAsync(fooIProgressDialog);
                    var fooResult = await LoginUpdateTokenHelper.UserLogoutAsync(dialogService, loginManager, systemStatusManager, appStatus);
                    if (fooResult == true)
                    {
                        await navigationService.NavigateAsync("/LoginPage");
                    }
                }
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

    }
}
