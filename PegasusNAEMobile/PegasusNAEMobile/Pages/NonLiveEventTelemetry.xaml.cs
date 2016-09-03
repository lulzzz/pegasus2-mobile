using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PegasusData;
using Xamarin.Forms;
using PegasusNAEMobile.Collections;
namespace PegasusNAEMobile
{
    public partial class NonLiveEventTelemetry : ContentPage
    {
        private RootObjectOnboardTelemetry ronboardtelem;
        public NonLiveEventTelemetry()
        {
            InitializeComponent();
            ronboardtelem = new RootObjectOnboardTelemetry();
            vehicle.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("pegasus_vehicle_small.png"),
            Android: ImageSource.FromFile("pegasus_vehicle_small.png"),
            WinPhone: ImageSource.FromFile("Assets/pegasus_vehicle_small.png"));
            System.Diagnostics.Debug.WriteLine(Constants.ScreenHeight + ", " + Constants.ScreenWidth);    
        }

        protected async override void OnAppearing()
        {
            string onboardtelemetry = await App.Instance.GetLaunchInfoAsync();
            RootObjectOnboardTelemetry rtelem = OnboardTelemetryCollection.DataDeserializer(onboardtelemetry);
            System.Diagnostics.Debug.WriteLine(rtelem.collection.Count);
            base.OnAppearing();
        }
        private void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            
        }       
    }
}
