using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Healthband.Classes;
using Healthband.Pages;



namespace Healthband.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageAfterNewAcc : ContentPage
    {
        public LoginPageAfterNewAcc()
        {
            InitializeComponent();
            BtnLogin = new Button();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            string UserEmail = entryEmail.Text;
            string UserPassword = entryPassword.Text;

            try
            {
                UserManager manager = new UserManager();
                IEnumerable<UserLoginResponse> response = await manager.Login_user(UserEmail, UserPassword);
                string res = response.First().Response;
                if (res == "OK")
                {
                    App._id = response.First().Id_user;
                    await Navigation.PushModalAsync(new MenuPage(response.First().Id_user));
                }
                else if (res == "EMAIL_ERROR")
                {
                    await DisplayAlert("Alert", "Utilizador Errado", "OK");
                    entryEmail.Text = "";
                    entryPassword.Text = "";
                    App._id = 0;
                }
                else if (res == "PASSWORD_ERROR")
                {
                    await DisplayAlert("Alert", "Password Errada", "OK");
                    entryEmail.Text = "";
                    entryPassword.Text = "";
                    App._id = 0;
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine("ERRO - - - - " + e1);
            }
        }
    }
}