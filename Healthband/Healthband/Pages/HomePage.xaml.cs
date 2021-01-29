using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OpenWindesheart;
using OpenWindesheart.Models;

namespace Healthband.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            App.RequestLocationPermission();

            try
            {
                if (Windesheart.PairedDevice.IsConnected())
                {
                    ReadCurrentSteps();
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO - - - - " + e);
            }



        }

        public async void ReadCurrentSteps()
        {
            var steps = await Windesheart.PairedDevice.GetSteps();
            UpdateSteps(steps);
        }


        public void UpdateSteps(StepData steps)
        {
            if(steps.StepCount == 0)
            {
                Device.BeginInvokeOnMainThread(delegate {
                    LabelSteps.Text = "00000";
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(delegate {
                    LabelSteps.Text = steps.StepCount.ToString();
                });
            }
        }

        private void ReloadPage_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Windesheart.PairedDevice.IsConnected())
                {
                    ReadCurrentSteps();
                }
                else
                {
                    return;
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine("ERRO - - - - " + e1);
            }
        }
    }
}