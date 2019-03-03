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
    using LOBApp.Common.Helpers.ManagerHelps;
    using LOBApp.Models;
    using LOBApp.Common.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using LOBApp.Common.DTOs;
    using LOBApp.Common.Models;

    public class LeaveFormDetailPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public LeaveFormModel LeaveFormSelectedItem { get; set; }
        public LeaveFormItemModel LeaveFormItemModel { get; set; } = new LeaveFormItemModel();
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly LeaveFormsManager leaveFormsManager;
        private readonly LeaveFormTypesManager leaveFormTypesManager;
        public bool IsAddMode { get; set; }
        public bool IsEditMode { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public ObservableCollection<LeaveFormTypeModel> LeaveFormTypeItemsSource { get; set; } = new ObservableCollection<LeaveFormTypeModel>();
        public LeaveFormTypeModel LeaveFormTypeSelectedItem { get; set; }

        public LeaveFormDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            RefreshTokenManager refreshTokenManager,
            SystemStatusManager systemStatusManager, AppStatus appStatus,
            LeaveFormsManager leaveFormsManager, LeaveFormTypesManager leaveFormTypesManager)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.leaveFormsManager = leaveFormsManager;
            this.leaveFormTypesManager = leaveFormTypesManager;
            SaveCommand = new DelegateCommand(async () =>
            {
                if (IsAddMode == true)
                {
                    using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
                    {
                        bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
                        if (fooRefreshTokenResult == false)
                        {
                            return;
                        }
                        var fooResult = await leaveFormsManager.PostAsync(new LeaveFormRequestDTO()
                        {
                            id = 0,
                            BeginTime = LeaveFormItemModel.BeginDate + LeaveFormItemModel.BeginTime,
                            EndTime = LeaveFormItemModel.EndDate + LeaveFormItemModel.EndTime,
                            Description = LeaveFormItemModel.Description,
                            TotalHours = LeaveFormItemModel.TotalHours,
                            leaveFormType = new LeaveFormTypeDTO() { Id = LeaveFormTypeSelectedItem.Id }
                        });
                    }
                }
                else
                {
                    using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
                    {
                        bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
                        if (fooRefreshTokenResult == false)
                        {
                            return;
                        }
                        var fooResult = await leaveFormsManager.PutAsync(LeaveFormSelectedItem.Id,new LeaveFormRequestDTO()
                        {
                            id = LeaveFormSelectedItem.Id,
                            BeginTime = LeaveFormItemModel.BeginDate + LeaveFormItemModel.BeginTime,
                            EndTime = LeaveFormItemModel.EndDate + LeaveFormItemModel.EndTime,
                            Description = LeaveFormItemModel.Description,
                            TotalHours = LeaveFormItemModel.TotalHours,
                            leaveFormType = new LeaveFormTypeDTO() { Id = LeaveFormTypeSelectedItem.Id }
                        });
                    }
                }
                var queryString = "NeedRefresh=true";
                var navigationParams = new NavigationParameters(queryString);
                await navigationService.GoBackAsync(navigationParams);
            });
            DeleteCommand = new DelegateCommand(async () =>
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
                {
                    bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
                    if (fooRefreshTokenResult == false)
                    {
                        return;
                    }
                    var fooResult = await leaveFormsManager.DeleteAsync(LeaveFormSelectedItem.Id);
                }
                var queryString = "NeedRefresh=true";
                var navigationParams = new NavigationParameters(queryString);
                await navigationService.GoBackAsync(navigationParams);
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await leaveFormTypesManager.ReadFromFileAsync();
            LeaveFormSelectedItem = parameters.GetValue<LeaveFormModel>("Record");
            if (LeaveFormSelectedItem.Id == 0)
            {
                IsAddMode = true;
                IsEditMode = false;
            }
            else
            {
                IsAddMode = false;
                IsEditMode = true;
            }
            LeaveFormItemModel.BeginDate = LeaveFormSelectedItem.BeginTime.Date;
            LeaveFormItemModel.BeginTime = LeaveFormSelectedItem.BeginTime.TimeOfDay;
            LeaveFormItemModel.EndDate = LeaveFormSelectedItem.EndTime.Date;
            LeaveFormItemModel.EndTime = LeaveFormSelectedItem.BeginTime.TimeOfDay;
            LeaveFormItemModel.Description = LeaveFormSelectedItem.Description;
            LeaveFormItemModel.TotalHours = LeaveFormSelectedItem.TotalHours;

            foreach (var item in leaveFormTypesManager.Items)
            {
                LeaveFormTypeModel leaveFormTypeModel = new LeaveFormTypeModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                };
                LeaveFormTypeItemsSource.Add(leaveFormTypeModel);
            }

            var fooItem = LeaveFormTypeItemsSource.FirstOrDefault(x => x.Id == LeaveFormSelectedItem.leaveFormType.Id);
            if (fooItem != null)
            {
                LeaveFormTypeSelectedItem = fooItem;
            }
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

    }
}
