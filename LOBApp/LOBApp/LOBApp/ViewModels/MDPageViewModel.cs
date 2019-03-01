using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LOBApp.ViewModels
{
    using System.ComponentModel;
    using LOBApp.Helpers.ManagerHelps;
    using LOBApp.Models;
    using LOBApp.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    public class MDPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly LoginManager loginManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;

        public DelegateCommand UserGroupCommand { get; set; }
        public DelegateCommand LeaveFormCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }
        public MDPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            LoginManager loginManager, SystemStatusManager systemStatusManager,
            AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.loginManager = loginManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            UserGroupCommand = new DelegateCommand(async () =>
            {

            });
            LeaveFormCommand = new DelegateCommand(async () =>
            {

            });
            LogoutCommand = new DelegateCommand(async () =>
            {
                var fooResult = await LoginUpdateTokenHelper.UserLogoutAsync(dialogService, loginManager, systemStatusManager,
                    appStatus);
                if (fooResult == true)
                    await navigationService.NavigateAsync("/LoginPage");
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
