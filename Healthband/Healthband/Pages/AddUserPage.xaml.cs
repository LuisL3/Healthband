using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Healthband.Classes;

namespace Healthband.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUserPage : ContentPage
    {
        public AddUserPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        private async void BtnRegister_Clicked(object sender, EventArgs e)
        {
            string name = entryName.Text;
            string email = entryEmail.Text;
            string password = entryPassword.Text;
            string confim_password = entryConfirmPassword.Text;

            if (password != confim_password)
            {
                await DisplayAlert("Aviso", "As password´s que introduziu são diferentes.\nInsira novamente.", "OK");
                entryPassword.Text = "";
                entryConfirmPassword.Text = "";
            }
            else
            {
                if (entryEmail.Text != null && entryName.Text != null && entryPassword.Text != null && entryConfirmPassword.Text != null)
                { 
                    try
                    {
                        UserManager manager = new UserManager();
                        //Envia os dados de registo para a Classe UserManager
                        manager.addUser(name, password, email);
                        await Navigation.PushModalAsync(new LoginPageAfterNewAcc());
                    }
                    catch (Exception e1)
                    {
                        Console.WriteLine("ERRO - - - - - " + e1);
                    }
                }
                else
                {
                    await DisplayAlert("Aviso", "Campos obrigatórios por preencher.", "OK");
                }
            }
        }
    }
}