using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PegasusNAEMobile.ViewModels;
using Xamarin.Forms;
using PegasusNAEMobile.ViewModels.Pages;
using System.Globalization;

namespace PegasusNAEMobile
{
    public partial class LiveEventTelemetry : ContentPage
    {
        private double width = 0;
        private double height = 0;
        public LiveEventTelemetry()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            //sendMessageButton.Image = ImageSource.FromFile("Assets/pegasus_herobackground.png");
            sendMessageButton.Image = "Assets/Send.png";
            SizeChanged += LiveEventTelemetry_SizeChanged;
            //UserMessageSentStatus.FadeTo(0, 0);
            if (Device.OS == TargetPlatform.iOS)
            {
                BackButton.Image = "Back.png";
                sendMessageButton.Image = "send.png";
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                BackButton.Image = "back.png";
                sendMessageButton.Image = "send.png";
            }
            else
            {
                BackButton.Image = "Assets/" + "Back.png";
                sendMessageButton.Image = "Assets/Send.png";
            }
            SizeChanged += LiveEventTelemetry_SizeChanged1;
        }

        private void LiveEventTelemetry_SizeChanged1(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            double fontsizeLarge = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            double fontsizeMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            double fontSizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            AtmosphericLabel.FontSize = fontsizeMedium;
            LinearAccelerationLabel.FontSize = fontsizeMedium;
            CompassDirection.FontSize = fontsizeLarge;
            DirectionLabel.FontSize = fontsizeMedium;
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
                    Padding = new Thickness(0, 0, 0, 0);
                }
                else
                {
                    Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
                }
            }
        }

        private void LiveEventTelemetry_SizeChanged(object sender, EventArgs e)
        {
            double fontsizeLarge = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            double fontsizeMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            double fontSizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            PageTitle.FontSize = fontsizeMedium;
            RunTimeStamp.FontSize = fontsizeMedium;
            RunID.FontSize = fontSizeSmall;
            SpeedGridTitle.FontSize = fontSizeSmall;
            SpeedMPH.FontSize = fontsizeLarge;            
            SpeedKPH.FontSize = fontsizeLarge;            
            SoundLevelLabel.FontSize = fontSizeSmall;
            SoundLevel.FontSize = fontsizeLarge;
            LinearAccelerationLabel.FontSize = fontSizeSmall;
            OrientationLabel.FontSize = fontSizeSmall;
            Orientation.FontSize = fontsizeMedium;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void sendMessageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await UserMessageSentStatus.FadeTo(0, 0);   // hide it
                sendMessageButton.IsEnabled = false;
                await App.Instance.SendUserMessageAsync(NAEUserMessage.Text);
                UserMessageSentStatus.Text = "Your message was sent successfully";   //change the message
                await UserMessageSentStatus.FadeTo(1, 500);   // make it visible over 0.5 seconds
                startTimer(1);
            }
            catch (Exception ex)
            {
                await UserMessageSentStatus.FadeTo(0, 0);
                UserMessageSentStatus.Text = "There was a problem sending your message";
                await UserMessageSentStatus.FadeTo(1, 500);
                startTimer(2);
            }
        }

        private void startTimer(int seconds)
        {
            TimeSpan ts = TimeSpan.FromSeconds(seconds);
            try
            {
                Device.StartTimer(ts, () => { UserMessageSentStatus.FadeTo(0, 300); UserMessageSentStatus.Text = "40 characters remaining"; UserMessageSentStatus.FadeTo(1, 0); return true; });
                sendMessageButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                UserMessageSentStatus.FadeTo(0, 200);
                sendMessageButton.IsEnabled = true;
                UserMessageSentStatus.Text = "40 characters remaining";
                UserMessageSentStatus.FadeTo(1, 0);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new TelemetryViewModel();        
        }

        private void NAEUserMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            string val = entry.Text;
            UserMessageSentStatus.Text = (40 - val.Length).ToString() + " characters remaining.";
            if (val.Length > 40)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }
        }

    }

    public class RoundToDecimalPlaces : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double round =  (Math.Round((double)value, 3, MidpointRounding.AwayFromZero));
            return (String.Format("{0:0.000}", round));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HeadingToCompassDirection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double heading = (double)value;
            var directions = new string[] { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
            int degreesPerDirection = (int)(360 / (directions.Length - 1));
            int index = (int)((heading + (degreesPerDirection / 2)) / degreesPerDirection);
            return directions[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
