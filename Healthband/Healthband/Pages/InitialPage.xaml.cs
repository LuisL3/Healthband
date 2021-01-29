using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Healthband.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialPage : ContentPage
    {
        public InitialPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage());
        }

        private  async void ResgiterBtn_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new AddUserPage());
        }
    }
}