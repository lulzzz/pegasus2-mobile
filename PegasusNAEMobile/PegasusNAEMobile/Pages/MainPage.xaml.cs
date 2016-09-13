using PegasusData;
using PegasusNAEMobile.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public partial class MainPage : ContentPage
    {
        private double height = 0;
        private double width = 0;
        public MainPage()
        {
            InitializeComponent();
            MainGrid.BackgroundColor = Color.Black;
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = new MainPageViewModel();
            pegasus_HeroBackground.Source = Device.OnPlatform(
                iOS: ImageSource.FromFile("pegasus_herobackground.png"),
                Android: ImageSource.FromFile("pegasus_herobackground.png"),
                WinPhone: ImageSource.FromFile("Assets/pegasus_herobackground.png"));
            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                pegasus_HeroBackground.IsVisible = true;
            }
            SizeChanged += MainPage_SizeChanged;
        }

        private void MainPage_SizeChanged(object sender, EventArgs e)
        {
            
            //RegisterForEventNotifications.FontSize = fontSizeSmall;
            
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;

                if (width > height)
                {
                    double fontsizeLarge = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    
                    fontsizeLarge = (int)(this.Height / 14);
                    double fontsizeMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    double fontSizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                    PageTitle.FontSize = fontsizeMedium;
                    HeroTitle.FontSize = fontsizeLarge;
                    Padding = new Thickness(0, 0, 0, 0);
                    Grid.SetColumn(HeroTitle, 0);
                    Grid.SetColumnSpan(HeroTitle, 1);
                    Grid.SetRow(HeroTitle, 0);
                    Grid.SetRowSpan(HeroTitle, 2);

                    Grid.SetColumn(ButtonLayout, 1);
                    Grid.SetColumnSpan(ButtonLayout, 1);
                    Grid.SetRow(ButtonLayout, 0);
                    Grid.SetRowSpan(ButtonLayout, 2);

                    RegisterForEventNotifications.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
                    WatchEventButton.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
                    WatchPreviousRuns.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
                    HeroTitle.WidthRequest = (int)((Constants.ScreenWidth) * 0.5);
                }
                else
                {
                    double fontsizeLarge = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    
                    fontsizeLarge = (int)(this.height / 14);
                    double fontsizeMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    double fontSizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                    PageTitle.FontSize = fontsizeMedium;
                    HeroTitle.FontSize = fontsizeLarge;
                    Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
                    Grid.SetRow(HeroTitle, 0);
                    Grid.SetRowSpan(HeroTitle, 1);
                    Grid.SetColumn(HeroTitle, 0);
                    Grid.SetColumnSpan(HeroTitle, 2);
                    Grid.SetRow(ButtonLayout, 1);
                    Grid.SetRowSpan(ButtonLayout, 1);
                    Grid.SetColumn(ButtonLayout, 0);
                    Grid.SetColumnSpan(ButtonLayout, 2);
                    RegisterForEventNotifications.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
                    WatchEventButton.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
                    WatchPreviousRuns.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
                    HeroTitle.WidthRequest = (int)((Constants.ScreenWidth) * 0.8);
                }
            }
        }

        private void RegisterForEventNotifications_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://www.pegasusmission.io/Home/Notifications");
            Device.OpenUri(uri);
        }

        private async void WatchLiveEvent_Clicked(object sender, EventArgs e)
        {
            Constants.SubscribedSuccessfully = false;
            App.Instance.ConnectWebSocketLiveTelemetry();
            //var LiveEventTelemetryPage = new LiveEventTelemetry();
            //LiveEventTelemetryPage.BindingContext = App.Instance.CurrentVehicleTelemetry;
            await Navigation.PushAsync(new LiveEventTelemetry());
        }

        private async void AboutButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new About());
        }

        private async void WatchPreviousRuns_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PreviousRunsList());
        }       
    }
}
