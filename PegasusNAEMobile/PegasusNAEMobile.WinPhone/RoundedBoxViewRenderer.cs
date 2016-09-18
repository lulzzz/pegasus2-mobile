using PegasusNAEMobile;
using PegasusNAEMobile.WinPhone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms.Platform.WinRT;

[assembly: ExportRendererAttribute(typeof(RoundedBoxView), typeof(RoundedBoxViewRenderer))]
namespace PegasusNAEMobile.WinPhone
{
    public class RoundedBoxViewRenderer : ViewRenderer<RoundedBoxView, Ellipse>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<RoundedBoxView> e)
        {
            base.OnElementChanged(e);
            try
            {
                var ellipse = new Ellipse();
                ellipse.Height = this.Height;
                ellipse.Width = this.Width;
                ellipse.StrokeThickness = 3;
                ellipse.Stroke = new SolidColorBrush(Colors.Red);
                this.SetNativeControl(ellipse);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
