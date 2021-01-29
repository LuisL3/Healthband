using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.BluetoothLE;

using OpenWindesheart;
using OpenWindesheart.Models;
using Rectangle = Xamarin.Forms.Rectangle;

namespace Healthband.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionPage : ContentPage
    {
        public static Picker StepsPicker;
        public static Picker HourPicker;
        public static Picker DatePicker;
        private int _stepIndex = 0;
        private int _hourIndex = 0;
        private int _dateIndex = 0;

        public ConnectionPage()
        {
            InitializeComponent();
            DrawPicker();
            DrawHourPicker();
            DrawDatePiker();
        }


        //SCAN
        private async void BtnScan_Clicked(object sender, EventArgs e)
        {

            if (Windesheart.IsScanning())
            {
                Windesheart.StopScanning();
                Device.BeginInvokeOnMainThread(delegate {
                    StatusText.Text = "Inicie a procura.";
                });
            }
            else
            {
                if (Windesheart.AdapterStatus == AdapterStatus.PoweredOff)
                {
                    await Application.Current.MainPage.DisplayAlert("Bluetooth está desativado",
                        "Por favor ative o Bluetooth para iniciar a procura.", "OK");
                    Device.BeginInvokeOnMainThread(delegate {
                        StatusText.Text = "Bluetooth desativado.";
                    });
                    return;
                }
            }

            if (Windesheart.StartScanning(WhenDeviceFound))
            {
                StatusText.Text = "A Procurar...";
                Console.WriteLine("Scanning started!");
                BtnStopScan.IsEnabled = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(delegate {
                    StatusText.Text = "Não foi possivel iniciar a procura.";
                });

                await Task.Delay(1500);
                Windesheart.StopScanning();
                Console.WriteLine("Scanning Stopped!");
            }
        }

        public async void WhenDeviceFound(BLEScanResult result)
        {

            Device.BeginInvokeOnMainThread(delegate {
                StatusText.Text = "Dispositivo Encontrado.";
            });

            Console.WriteLine("Device found!");

            BLEDevice device = result.Device;
            int Rssi = result.Rssi;
            IAdvertisementData data = result.AdvertisementData;

            await Task.Delay(1500);
            Windesheart.StopScanning();
            try
            { 
                device.Connect(OnConnectionFinished);
            }
            catch(Exception e1)
            {
                Console.WriteLine("ERRO - - - -" + e1);
            }

        }
            
        void OnConnectionFinished(ConnectionResult result, byte[] secretKey)
        {
            if (result == ConnectionResult.Succeeded)
            {
                Console.WriteLine("Successful Connection!");
                Device.BeginInvokeOnMainThread(delegate {
                    StatusText.Text = "Ligado.";
                    BtnDisconnect.IsEnabled = true;
                    BtnScan.IsEnabled = false;
                    BtnStopScan.IsEnabled = false;
                });
                if (Windesheart.PairedDevice.IsConnected())
                {
                    ReadCurrentBattery();
                }
                //SaveKeyToProperties(secretKey);
            }
            else
            {
                Console.WriteLine("Connection failed... :(");
            }
        }


        public void OnDisappearing()
        {
            Windesheart.StopScanning();
        }

        protected override void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(delegate {
                BtnStopScan.IsEnabled = false;
            });
            if (Windesheart.PairedDevice != null)
            {
                BtnDisconnect.IsEnabled = Windesheart.PairedDevice.IsConnected();
                ReadCurrentBattery();
            }
            else
            {
                BtnDisconnect.IsEnabled = false;
            }
        }

        private void BtnStopScan_Clicked(object sender, EventArgs e)
        {
            Windesheart.StopScanning();
            Device.BeginInvokeOnMainThread(delegate {
                StatusText.Text = "Procura cancelada.";
                BtnStopScan.IsEnabled = false;
            });

        }

        private void BtnDisconnect_Clicked(object sender, EventArgs e)
        {
            BLEDevice device = Windesheart.PairedDevice;
            device.Disconnect(true);

            Device.BeginInvokeOnMainThread(delegate {
                StatusText.Text = "Desconectado.";
                BtnScan.IsEnabled = true;
            });
        }

        #region Ler Bateria
        public async Task ReadCurrentBattery()
        {
            //catch!!
            var battery = await Windesheart.PairedDevice.GetBattery();
            UpdateBattery(battery);
        }

        public void UpdateBattery(BatteryData battery)
        {
            if(battery.Status == 0)
            {
                Device.BeginInvokeOnMainThread(delegate {
                    BatteryStatus.Text = "A carregar..." + " - " + battery.Percentage + "%";
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(delegate {
                    BatteryStatus.Text = "Não está a carregar." + " - " + battery.Percentage + "%";
                });
            }
        }
        #endregion

        #region Passos
        void DrawPicker()
        {
            StepsPicker = new Picker { Title = "Nº de Passos", TextColor = Color.Black, FontSize = Height / 100 * 2.5, TitleColor = Color.Black };
            for (int i = 1; i < 21; i++)
                StepsPicker.Items.Add((i * 1000).ToString());

            StepsPicker.SelectedIndexChanged += StepsIndexChanged;
            AbsoluteLayout.SetLayoutBounds(StepsPicker, new Rectangle(.75, .36, 135, -1));
            AbsoluteLayout.SetLayoutFlags(StepsPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(StepsPicker);
        }

        public void StepsIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is Picker picker) || picker.SelectedIndex == -1) return;
            int steps = int.Parse(picker.Items[picker.SelectedIndex]);
            try
            {
                if (Windesheart.PairedDevice != null)
                {
                    if (Windesheart.PairedDevice.IsConnected())
                    {
                        Windesheart.PairedDevice.SetStepGoal(steps);
                        _stepIndex = picker.SelectedIndex;
                    }
                }
            }
            catch (Exception)
            {
                //Set picker index back to old value
                picker.SelectedIndex = _stepIndex;
                Console.WriteLine("Something went wrong!");
            }
        }
        #endregion

        private void SwitchRist_Toggled(object sender, ToggledEventArgs e)
        {
            bool toggled = SwitchRist.IsToggled;

            try
            {
                if (Windesheart.PairedDevice == null) return;
                if (!Windesheart.PairedDevice.IsConnected()) return;
                Windesheart.PairedDevice.SetActivateOnLiftWrist(toggled);
            }
            catch (Exception e1)
            {
                SwitchRist.IsToggled = !toggled;
                Console.WriteLine("ERRO - - - " + e1);
            }
        }

        #region Formato da hora
        void DrawHourPicker()
        {
            HourPicker = new Picker { Title = "Definir Formato", FontSize = Height / 100 * 2.5, TextColor = Color.Black, TitleColor = Color.Black };
            HourPicker.Items.Add("24 hour");
            HourPicker.Items.Add("12 hour");
            HourPicker.SelectedIndexChanged += HourIndexChanged;

            AbsoluteLayout.SetLayoutBounds(HourPicker, new Rectangle(0.75, 0.42, 135, -1));
            AbsoluteLayout.SetLayoutFlags(HourPicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(HourPicker);
        }

        public void HourIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is Picker picker) || picker.SelectedIndex == -1) return;
            string format = picker.Items[picker.SelectedIndex];
            bool is24 = format.Equals("24 hour");

            try
            {
                if (Windesheart.PairedDevice != null)
                {
                    if (Windesheart.PairedDevice.IsConnected())
                    {
                        Windesheart.PairedDevice.SetTimeDisplayFormat(is24);
                        _hourIndex = picker.SelectedIndex;
                    }
                }
            }
            catch (Exception)
            {
                picker.SelectedIndex = _hourIndex;
                Console.WriteLine("Something went wrong!");
            }
        }
        #endregion

        #region Formato da data
        void DrawDatePiker()
        {
            DatePicker = new Picker { FontSize = Height / 100 * 2.5, Title = "Definir Formato", TextColor = Color.Black, TitleColor = Color.Black };
            DatePicker.Items.Add("DD/MM/YYYY");
            DatePicker.Items.Add("MM/DD/YYYY");

            DatePicker.SelectedIndexChanged += DateIndexChanged;

            AbsoluteLayout.SetLayoutBounds(DatePicker, new Rectangle(0.75, 0.49, 135, -1));
            AbsoluteLayout.SetLayoutFlags(DatePicker, AbsoluteLayoutFlags.PositionProportional);
            absoluteLayout.Children.Add(DatePicker);
        }

        public void DateIndexChanged(object sender, EventArgs args)
        {
            if (!(sender is Picker picker) || picker.SelectedIndex == -1) return;
            string format = picker.Items[picker.SelectedIndex];
            bool isDMY = format.Equals("DD/MM/YYYY");

            try
            {
                if (Windesheart.PairedDevice != null)
                {
                    if (Windesheart.PairedDevice.IsConnected())
                    {
                        Windesheart.PairedDevice.SetDateDisplayFormat(isDMY);
                        _dateIndex = picker.SelectedIndex;
                    }
                }
            }
            catch (Exception)
            {
                //Set picker index back to old value
                picker.SelectedIndex = _dateIndex;
                Console.WriteLine("Something went wrong!");
            }
        }
        #endregion

    }
}