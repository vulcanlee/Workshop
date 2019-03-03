using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LOBApp.ViewModels
{
    using System.ComponentModel;
    using LOBApp.Models;
    using LOBApp.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using Xamarin.Forms;

    public class HomePageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string AppName { get; set; }
        public string AndroidVersion { get; set; }
        public string AndroidUrl { get; set; }
        public string iOSVersion { get; set; }
        public string iOSUrl { get; set; }
        public bool IsAndroidPlatform { get; set; } = true;
        public bool IsiOSPlatform { get; set; } = false;
        public string UserName { get; set; }

        private readonly INavigationService navigationService;
        private readonly SystemEnvironmentsManager systemEnvironmentsManager;
        private readonly AppStatus appStatus;

        public HomePageViewModel(INavigationService navigationService,
            SystemEnvironmentsManager systemEnvironmentsManager, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.systemEnvironmentsManager = systemEnvironmentsManager;
            this.appStatus = appStatus;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                IsAndroidPlatform = true;
                IsiOSPlatform = false;
            }
            else
            {
                IsAndroidPlatform = false;
                IsiOSPlatform = true;
            }
            await systemEnvironmentsManager.ReadFromFileAsync();
            AppName = systemEnvironmentsManager.SingleItem.AppName;
            AndroidVersion = systemEnvironmentsManager.SingleItem.AndroidVersion;
            AndroidUrl = systemEnvironmentsManager.SingleItem.AndroidUrl;
            iOSVersion = systemEnvironmentsManager.SingleItem.iOSVersion;
            iOSUrl = systemEnvironmentsManager.SingleItem.iOSUrl;

            UserName = appStatus.SystemStatus.Account;
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

    }
}
