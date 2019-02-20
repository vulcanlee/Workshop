using Prism;
using Prism.Ioc;
using LOBApp.ViewModels;
using LOBApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LOBApp.Services;
using LOBApp.Helpers.ManagerHelps;
using LOBApp.Models;
using System;
using LOBApp.DTOs;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LOBApp
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    var foo = 1;
                };

            await NavigationService.NavigateAsync("/SplashPage");
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<SystemStatusManager>();
            containerRegistry.Register<LoginManager>();
            containerRegistry.Register<DepartmentsManager>();
            containerRegistry.Register<CommUserGroupItemsManager>();
            containerRegistry.Register<CommUserGroupsManager>();
            containerRegistry.Register<ExceptionRecordsManager>();
            containerRegistry.Register<LeaveFormsManager>();
            containerRegistry.Register<LeaveFormTypesManager>();
            containerRegistry.Register<NotificationTokensManager>();
            containerRegistry.Register<SuggestionsManager>();
            containerRegistry.Register<SystemEnvironmentsManager>();
            containerRegistry.Register<RefreshTokenManager>();
            containerRegistry.Register<RecordCacheHelper>();
            containerRegistry.RegisterSingleton<AppStatus>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SamplePage, SamplePageViewModel>();
            containerRegistry.RegisterForNavigation<SplashPage, SplashPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
        }
    }
}
