using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using System;

namespace LOBApp.Droid
{
    [Activity(Label = "LOBApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            //{
            //    var foo = 1;
            //};
            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            #region 擴充套件初始化
            UserDialogs.Init(this);
            #endregion
            LoadApplication(new App(new AndroidInitializer()));
        }

        private async void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //ExceptionRecordsManager fooExceptionRecordsManager = new ExceptionRecordsManager();
            //await fooExceptionRecordsManager.ReadFromFileAsync();
            //ExceptionRecordResponseDTO fooObject = new ExceptionRecordResponseDTO()
            //{
            //    //CallStack = e.
            //};
        }
    }


    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

