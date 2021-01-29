using Healthband.Pages;
using Xamarin.Forms;
using Xamarin.Essentials;

using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace Healthband
{
    public partial class App : Application
    {
        public static int  _id { get; set; }
        public static bool IsloggedIn;
        public App()
        {
            InitializeComponent();
            //MainPage = new NavigationPage(new InitialPage());
            MainPage = new NavigationPage(new LoginPage());
            _id = 0;
            IsloggedIn = false;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static async void RequestLocationPermission()
        {
            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
            }
        }
    }
}
