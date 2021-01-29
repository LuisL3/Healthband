using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Healthband.Classes;

namespace Healthband.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BtnLogin = new Button();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void BtnNewAccount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddUserPage());
            //await Navigation.PushAsync(new NavigationPage(new AddUserPage()));
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
                    App.IsloggedIn = true;
                    await Navigation.PushModalAsync(new MenuPage(response.First().Id_user));
                }
                else if (res == "EMAIL_ERROR")
                {
                    await DisplayAlert("Alert", "Utilizador Errado", "OK");
                    entryEmail.Text = "";
                    entryPassword.Text = "";
                    App._id = 0;
                    App.IsloggedIn = false;
                }
                else if (res == "PASSWORD_ERROR")
                { 
                    await DisplayAlert("Alert", "Password Errada", "OK");
                    entryEmail.Text = "";
                    entryPassword.Text = "";
                    App._id = 0;
                    App.IsloggedIn = false;
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine("ERRO - - - - " + e1);
            }
        }
    }
}