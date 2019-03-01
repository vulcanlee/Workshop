using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LOBApp.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Acr.UserDialogs;
    using LOBApp.DTOs;
    using LOBApp.Models;
    using LOBApp.Services;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    public class CommUsePageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<CommUserGroupModel> CommUserItemsSource { get; set; } = new ObservableCollection<CommUserGroupModel>();
        public ObservableCollection<CommUserGroupItemModel> CommUserItemItemsSource { get; set; } = new ObservableCollection<CommUserGroupItemModel>();
        public CommUserGroupModel CommUserSelectedItem { get; set; }
        public CommUserGroupItemModel CommUserItemSelectedItem { get; set; }
        private readonly INavigationService navigationService;
        private readonly CommUserGroupsManager commUserGroupsManager;
        private readonly CommUserGroupItemsManager commUserGroupItemsManager;

        public CommUsePageViewModel(INavigationService navigationService,
            CommUserGroupsManager commUserGroupsManager, CommUserGroupItemsManager commUserGroupItemsManager)
        {
            this.navigationService = navigationService;
            this.commUserGroupsManager = commUserGroupsManager;
            this.commUserGroupItemsManager = commUserGroupItemsManager;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnCommUserSelectedItemChanged()
        {
            if (CommUserSelectedItem != null)
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
                {
                   var fooResult= await commUserGroupItemsManager.PostAsync(new CommUserGroupItemRequestDTO()
                    {
                        Id = CommUserSelectedItem.Id
                    });
                    CommUserItemItemsSource.Clear();
                    foreach (var item in commUserGroupItemsManager.Items)
                    {
                        CommUserGroupItemModel commUserGroupItemModel = new CommUserGroupItemModel()
                        {
                            Id = item.Id,
                            Email = item.Email,
                            Name = item.Name,
                            Mobile = item.Mobile,
                            Phone = item.Phone,
                        };
                        CommUserItemItemsSource.Add(commUserGroupItemModel);
                    }
                }
            }
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await commUserGroupsManager.ReadFromFileAsync();
            CommUserItemsSource.Clear();
            foreach (var item in commUserGroupsManager.Items)
            {
                CommUserGroupModel commUserGroupModel = new CommUserGroupModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                };
                CommUserItemsSource.Add(commUserGroupModel);
            }
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }

    }
}
