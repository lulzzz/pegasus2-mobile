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
        public LiveEventTelemetry()
        {
            InitializeComponent();
            Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 0);
            //sendMessageButton.Image = ImageSource.FromFile("Assets/pegasus_herobackground.png");
            sendMessageButton.Image = "Assets/Send.png";
            SizeChanged += LiveEventTelemetry_SizeChanged;
            UserMessageSentStatus.FadeTo(0, 0);
        }

        private void LiveEventTelemetry_SizeChanged(object sender, EventArgs e)
        {
            double fontsizeLarge = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            double fontsizeMedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            double fontSizeSmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            RunTimeStamp.FontSize = fontsizeMedium;
            RunID.FontSize = fontSizeSmall;
            SpeedGridTitle.FontSize = fontsizeMedium;
            SpeedMPH.FontSize = fontsizeLarge;            
            SpeedKPH.FontSize = fontsizeLarge;            
            SoundLevelLabel.FontSize = fontsizeMedium;
            SoundLevel.FontSize = fontsizeLarge;
        }

        private async void sendMessageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                sendMessageButton.IsEnabled = false;
                await App.Instance.SendUserMessageAsync(NAEUserMessage.Text);
                UserMessageSentStatus.Text = "Your message was sent successfully";
                await UserMessageSentStatus.FadeTo(1, 500);
                startTimer(1);
            }
            catch (Exception ex)
            {
                UserMessageSentStatus.Text = "There was a problem sending your message";
                await UserMessageSentStatus.FadeTo(1, 500);
                startTimer(1);
            }
        }

        private void startTimer(int seconds)
        {
            TimeSpan ts = TimeSpan.FromSeconds(seconds);
            try
            {
                Device.StartTimer(ts, () => { UserMessageSentStatus.FadeTo(0, 300); return true; });
                sendMessageButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                UserMessageSentStatus.FadeTo(0, 200);
                sendMessageButton.IsEnabled = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new TelemetryViewModel();        
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
}
