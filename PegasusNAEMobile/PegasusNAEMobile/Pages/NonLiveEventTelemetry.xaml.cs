using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PegasusData;
using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public partial class NonLiveEventTelemetry : ContentPage
    {
        public NonLiveEventTelemetry()
        {
            InitializeComponent();
            vehicle.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("pegasus_vehicle.png"),
            Android: ImageSource.FromFile("pegasus_vehicle.png"),
            WinPhone: ImageSource.FromFile("Assets/pegasus_vehicle.png"));
            System.Diagnostics.Debug.WriteLine(Constants.ScreenHeight + ", " + Constants.ScreenWidth);
            if (Constants.ScreenHeight < 600)
            {
                vehicle.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("pegasus_vehicle_small.png"),
            Android: ImageSource.FromFile("pegasus_vehicle_small.png"),
            WinPhone: ImageSource.FromFile("Assets/pegasus_vehicle_small.png"));
            }
        }

        private void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            
        }

       
    }
}
