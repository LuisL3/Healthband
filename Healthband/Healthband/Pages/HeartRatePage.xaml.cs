using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Healthband.Classes;
using Entry = Microcharts.Entry;
using OpenWindesheart.Models;
using OpenWindesheart;
using System.Threading.Tasks;
using Healthband.Pages;

namespace Healthband.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeartRatePage : ContentPage
    {
        public async void CreateGraphic()
        {
            BpmManager manager = new BpmManager();
            IEnumerable<UserBpmResponse> response = await manager.SendBPM(App._id);
            List<Entry> entries = new List<Entry>();
            BpmManager bpms = new BpmManager();
            List<BPM> lista;

            lista =  new List<BPM>(await bpms.GetBPM());

            foreach (BPM a in lista){
                Entry no = new Entry(a.VALUE_BPM);
                no.ValueLabel = a.VALUE_BPM.ToString();
                no.Color = SKColor.Parse("#FF1943");
                no.Label = a.DATE_BPM.ToString();
                entries.Add(no);
            }


            Grafico_BPM.Chart = new LineChart() { LabelTextSize = 15f, Entries = entries };
            Grafico_BPM.WidthRequest = Application.Current.MainPage.Width;
            Grafico_BPM.HeightRequest = 200;
            Grafico_BPM.TranslationY = Grafico_BPM.TranslationY + 220;
        }

        public HeartRatePage()
        {
            InitializeComponent();
            CreateGraphic();
            BtnStartHeartrate.Clicked += BtnStartHeartrate_Clicked;
            BtnStopHeartrate.Clicked += BtnStopHeartrate_Clicked;
            BtnStopHeartrate.IsEnabled = false;
            BtnStartHeartrate.IsEnabled = false;
        }

        protected async override void OnAppearing()
        {

            if (Windesheart.PairedDevice == null)
            {
                await DisplayAlert("Aviso", "Nenhum dispositivo conectado.", "OK");
            }
            else
            {
                BtnStartHeartrate.IsEnabled = true;
                await ReadHeartrate();
            }
        }

        public async Task ReadHeartrate()
        {
            BLEDevice device = Windesheart.PairedDevice;
            try
            {
                if (device.IsConnected())
                {
                    device.EnableRealTimeHeartrate(OnHeartrateUpdate);
                    await Task.Delay(60000);
                    device.DisableRealTimeHeartrate();
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine("ERRO - - - - " + e1);
            }

        }

        public void OnHeartrateUpdate(HeartrateData data)
        {
            BpmManager manager = new BpmManager();
            DateTime thisDay = DateTime.Today;
            DateTime hour = DateTime.Now;
            int heartrate = data.Heartrate;

            Device.BeginInvokeOnMainThread(delegate {
                LabelHeartRate.Text = heartrate.ToString();
            });
            if(heartrate != 0)
                manager.addBPM(heartrate, thisDay, App._id);
        }
        private async void OnIntervalClicked(object sender, EventArgs e)
        {
            var intervalButton = sender as Button;
            BLEDevice device = Windesheart.PairedDevice;

            if (device.IsConnected())
            {
                if (intervalButton == null) return;

                var interval = Convert.ToInt32(intervalButton.Text);
                device.SetHeartrateMeasurementInterval(interval);
            }
            else
            {
                await DisplayAlert("Aviso", "Nenhum dispositivo conectado.", "OK");
            }
        }

        public async void BtnStartHeartrate_Clicked(object sender, EventArgs e)
        {
            BtnStopHeartrate.IsEnabled = true;
            try
            {
                await ReadHeartrate();
            }
            catch (Exception e1)
            {
                Console.WriteLine("ERRO - - - " + e1);
            }
        }

        public void BtnStopHeartrate_Clicked(object sender, EventArgs e)
        {
            BLEDevice device = Windesheart.PairedDevice;
            device.DisableRealTimeHeartrate();
        }
    }
}