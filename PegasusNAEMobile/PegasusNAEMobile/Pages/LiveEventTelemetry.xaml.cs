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
