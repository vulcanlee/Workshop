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
    using LOBApp.Common.DTOs;

    public class SuggestionPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SuggestionModel SuggestionModel { get; set; } = new SuggestionModel();
        private readonly INavigationService navigationService;
        private readonly IPageDialogService dialogService;
        private readonly RefreshTokenManager refreshTokenManager;
        private readonly SystemStatusManager systemStatusManager;
        private readonly AppStatus appStatus;
        private readonly SuggestionsManager suggestionsManager;

        public DelegateCommand OKCommand { get; set; }

        public SuggestionPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            RefreshTokenManager refreshTokenManager, SystemStatusManager systemStatusManager,
            AppStatus appStatus, SuggestionsManager suggestionsManager)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.refreshTokenManager = refreshTokenManager;
            this.systemStatusManager = systemStatusManager;
            this.appStatus = appStatus;
            this.suggestionsManager = suggestionsManager;
            OKCommand = new DelegateCommand(async () =>
            {
                using (IProgressDialog fooIProgressDialog = UserDialogs.Instance.Loading($"請稍後，更新資料中...", null, null, true, MaskType.Black))
                {
                    bool fooRefreshTokenResult = await RefreshTokenHelper.CheckAndRefreshToken(dialogService, refreshTokenManager, systemStatusManager, appStatus);
                    if (fooRefreshTokenResult == false)
                    {
                        return;
                    }
                    var fooResult = await suggestionsManager.PostAsync(new SuggestionRequestDTO()
                    {
                        Subject = SuggestionModel.Subject,
                        Message = SuggestionModel.Message,
                        SubmitTime = DateTime.Now,
                        User = new UserDTO() { Id = appStatus.SystemStatus.UserID },
                    });
                    if(fooResult.Status==true)
                    {
                        await dialogService.DisplayAlertAsync("通知", "已經成功提交", "確定");
                    }
                    else
                    {
                        await dialogService.DisplayAlertAsync("錯誤", $"已經發生例外異常:{fooResult.Message}", "確定");
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
