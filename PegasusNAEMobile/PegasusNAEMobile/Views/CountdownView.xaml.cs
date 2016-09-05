using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PegasusNAEMobile.Pages;
using PegasusData;

namespace PegasusNAEMobile
{
    public partial class CountdownView : ContentView
    {
        public CountdownView()
        {
            InitializeComponent();
            // asgangal : I was going to put this in CountdownViewModel and bind it as an Image Source, but I was doing something wrong there because the app kept crashing. We should fix this.
            pegasus_HeroBackground.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("pegasus_herobackground.png"),
            Android: ImageSource.FromFile("pegasus_herobackground.png"),
            WinPhone: ImageSource.FromFile("Assets/pegasus_herobackground.png"));
           
            SizeChanged += CountdownView_SizeChanged;
        }

        private void CountdownView_SizeChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            double fontsizeLarge = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            if (fontsizeLarge < 35)
                fontsizeLarge = 45;
            fontsizeLarge = (int) (Constants.ScreenHeight / 14);
            double fontsizeMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            double fontSizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            PageTitle.FontSize = fontsizeMedium;
            HeroTitle.FontSize = fontsizeLarge;
            //RegisterForEventNotifications.FontSize = fontSizeSmall;
            RegisterForEventNotifications.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
            WatchEventButton.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
            WatchPreviousRuns.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
            HeroTitle.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);            
        }

        private async void WatchLiveEvent_Clicked(object sender, EventArgs e)
        {
            App.Instance.ConnectWebSocketLiveTelemetry();
            //var LiveEventTelemetryPage = new LiveEventTelemetry();
            //LiveEventTelemetryPage.BindingContext = App.Instance.CurrentVehicleTelemetry;
            await Navigation.PushAsync(new LiveEventTelemetry());
        }

        private async void WatchPreviousRuns_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PreviousRunsList());
        }
    }
}
