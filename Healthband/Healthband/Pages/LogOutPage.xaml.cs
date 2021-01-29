using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Healthband.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogOutPage : ContentPage
    {
        public LogOutPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void YesBtn_Clicked(object sender, EventArgs e)
        {
            App._id = 0;
            Navigation.PushModalAsync(new LoginPage());
            Application.Current.MainPage = new LoginPage();
        }

        private void NoBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HomePage());
        }
    }
}