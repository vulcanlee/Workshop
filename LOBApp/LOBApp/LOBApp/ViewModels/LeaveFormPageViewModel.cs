using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LOBApp.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Acr.UserDialogs;
    using LOBApp.Helpers.ManagerHelps;
    using LOBApp.Models;
    using LOBApp.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    public class LeaveFormPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<LeaveFormModel> LeaveFormItemsSource { get; set; } = new ObservableCollection<LeaveFormModel>();
        public LeaveFormModel LeaveFormSelectedItem { get; set; }
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly LeaveFormsManager leaveFormsManager;
        private readonly LeaveFormTypesManager leaveFormTypesManager;
        private readonly RefreshTokenManager refreshTokenManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;
        public bool IsRefreshing { get; set; }

        public DelegateCommand ItemTappedCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand RefreshCommand { get; set; }

        public LeaveFormPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            LeaveFormsManager leaveFormsManager, LeaveFormTypesManager leaveFormTypesManager,
            RefreshTokenManager refreshTokenManager,
            SystemStatusManager systemStatusManager, AppStatus appStatus)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.leaveFormsManager = leaveFormsManager;
            this.leaveFormTypesManager = leaveFormTypesManager;
            this.refreshTokenManager = refreshTokenManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            RefreshCommand = new DelegateCommand(async () =>
            {
                IsRefreshing = true;
                await LoadDataAsync();
                IsRefreshing = false;
            });
            AddCommand = new DelegateCommand(async () =>
            {
                LeaveFormModel leaveFormModel = new LeaveFormModel()
                {
                    BeginTime = DateTime.Today + new TimeSpan(09, 00, 00),
                    EndTime = DateTime.Today + new TimeSpan(18, 00, 00),
                    Id = 0,
                    TotalHours = 8,
                    user = new DTOs.UserDTO() { Id = appStatus.SystemStatus.UserID },
                    Description = "請假原因",
                    leaveFormType = new LeaveFormTypeModel() { Id = leaveFormTypesManager.Items[0].Id, Name = leaveFormTypesManager.Items[0].Name },
                };
                NavigationParameters fooPara = new NavigationParameters();
                fooPara.Add("Record", leaveFormModel);
                await navigationService.NavigateAsync("LeaveFormDetailPage", fooPara);
            });
            ItemTappedCommand = new DelegateCommand(async () =>
            {
                var leaveFormSelectedItemClone = LeaveFormSelectedItem.Clone();
                NavigationParameters fooPara = new NavigationParameters();
                fooPara.Add("Record", leaveFormSelectedItemClone);
                await navigationService.NavigateAsync("LeaveFormDetailPage", fooPara);
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            var naviMode = parameters.GetNavigationMode();
            if (naviMode == NavigationMode.New)
            {
                await leaveFormTypesManager.ReadFromFileAsync();
                await leaveFormsManager.ReadFromFileAsync();
                await LoadDataAsync(false);
            }
            else
            {
                bool fooNeedRefresh = parameters.GetValue<bool>("NeedRefresh");
                if (fooNeedRefresh == true)
                    await LoadDataAsync();
            }
        }

        private async System.Threading.Tasks.Task LoadDataAsync(bool isDownload = true)
        {
            if (isDownload)
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
                {
                    bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
                    if (fooRefreshTokenResult == false)
                    {
                        return;
                    }
                    var fooResult = await leaveFormsManager.GetAsync();
                }
            }
            //await leaveFormsManager.ReadFromFileAsync();
            LeaveFormItemsSource.Clear();
            foreach (var item in leaveFormsManager.Items)
            {
                LeaveFormModel leaveFormModel = new LeaveFormModel()
                {
                    Id = item.Id,
                    user = new DTOs.UserDTO() { Id = item.user.Id },
                    BeginTime = item.BeginTime,
                    EndTime = item.EndTime,
                    TotalHours = item.TotalHours,
                    leaveFormType = new LeaveFormTypeModel() { Id = item.leaveFormType.Id },
                    Description = item.Description,
                };
                var fooItem = leaveFormTypesManager.Items.FirstOrDefault(x => x.Id == item.leaveFormType.Id);
                leaveFormModel.leaveFormType.Name = fooItem.Name;

                LeaveFormItemsSource.Add(leaveFormModel);
            }
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

    }
}
