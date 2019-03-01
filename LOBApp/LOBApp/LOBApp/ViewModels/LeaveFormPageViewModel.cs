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
        private readonly LeaveFormsManager leaveFormsManager;
        private readonly LeaveFormTypesManager leaveFormTypesManager;

        public LeaveFormPageViewModel(INavigationService navigationService,
            LeaveFormsManager leaveFormsManager, LeaveFormTypesManager leaveFormTypesManager)
        {
            this.navigationService = navigationService;
            this.leaveFormsManager = leaveFormsManager;
            this.leaveFormTypesManager = leaveFormTypesManager;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadDataAsync();
        }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            await leaveFormTypesManager.ReadFromFileAsync();
            using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
            {
                var fooResult = await leaveFormsManager.GetAsync();
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
