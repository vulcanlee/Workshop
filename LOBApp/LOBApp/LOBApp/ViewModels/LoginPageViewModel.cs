using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LOBApp.ViewModels
{
    using System.ComponentModel;
    using LOBApp.DTOs;
    using LOBApp.Helpers.ManagerHelps;
    using LOBApp.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
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

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            LoginManager loginManager, SystemStatusManager systemStatusManager)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.loginManager = loginManager;
            this.systemStatusManager = systemStatusManager;
            LoginCommand = new DelegateCommand(async() =>
            {
                LoginRequestDTO loginRequestDTO = new LoginRequestDTO()
                {
                    Account = Account,
                    Password = Password,
                };
                var fooResult = await LoginUpdateTokenHelper.UserLoginAsync(dialogService, loginManager, systemStatusManager,
                    loginRequestDTO);
                if (fooResult == false)
                    return;

                await dialogService.DisplayAlertAsync("Info", "登入成功", "OK");
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
