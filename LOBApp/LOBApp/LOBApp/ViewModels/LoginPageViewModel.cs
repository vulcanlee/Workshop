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
    using LOBApp.Models;
    using LOBApp.Common.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using LOBApp.Common.Models;

    public class LoginPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Account { get; set; }
        public string Password { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly LoginManager loginManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;
        private readonly RecordCacheHelper recordCacheHelper;

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            LoginManager loginManager, SystemStatusManager systemStatusManager,
            AppStatus appStatus, RecordCacheHelper recordCacheHelper)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.loginManager = loginManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            this.recordCacheHelper = recordCacheHelper;
            LoginCommand = new DelegateCommand(async () =>
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
                {
                    LoginRequestDTO loginRequestDTO = new LoginRequestDTO()
                    {
                        Account = Account,
                        Password = Password,
                    };
                    var fooResult = await LoginUpdateTokenHelper.UserLoginAsync(dialogService, loginManager, systemStatusManager,
                        loginRequestDTO, appStatus);
                    if (fooResult == false)
                        return;
                    await recordCacheHelper.RefreshAsync(fooIProgressDialog);
                }

                //await dialogService.DisplayAlertAsync("Info", "登入成功", "OK");
                await navigationService.NavigateAsync("/MDPage/NaviPage/HomePage");
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
#if DEBUG
            Account = "user1";
            Password = "password1";
#endif
            await systemStatusManager.ReadFromFileAsync();
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

    }
}
