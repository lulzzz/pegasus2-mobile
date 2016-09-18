using PegasusNAEMobile;
using PegasusNAEMobile.WinPhone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.WinRT;

[assembly: ExportRendererAttribute(typeof(ButtonView), typeof(ButtonViewRenderer))]
namespace PegasusNAEMobile.WinPhone
{
    class ButtonViewRenderer: ViewRenderer<ButtonView, Button>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ButtonView> e)
        {
            base.OnElementChanged(e);
            var buttonview = Element as ButtonView;
            if (e.OldElement == null && buttonview != null)
            {
                Button button = new Button();
                button.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe UI Symbol");
                button.Content = "\uE25D";
                button.Background = new SolidColorBrush(Colors.White);
                button.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
