using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LOBApp.ViewModels
{
    using System.ComponentModel;
    using LOBApp.Helpers.Storages;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using Xamarin.Essentials;

    public class SamplePageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DelegateCommand TestCommand { get; set; }
        private readonly INavigationService navigationService;

        public SamplePageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            TestCommand = new DelegateCommand(async () =>
            {
               await StorageUtility.WriteToDataFileAsync("MyFolder/Data", "MyFile.txt", "除了具備現代化語言 (C#) 的彈性和簡潔、.NET 基底類別庫 (BCL) 的強大功能，以及兩個頂級的 IDE (Visual Studio for Mac 和 Visual Studio) 之外，Xamarin.iOS 還可讓開發人員使用 Objective-C 和 Xcode 中可用的相同 UI 控制項來建立原生 iOS 應用程式。 此系列介紹如何設定及安裝 Xamarin.iOS，並說明 Xamarin.iOS 開發的基本概念。");
                var foo = await StorageUtility.ReadFromDataFileAsync("MyFolder/Data", "MyFile.txt");
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
