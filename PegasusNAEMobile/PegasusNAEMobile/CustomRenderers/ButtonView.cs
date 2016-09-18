using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PegasusNAEMobile.CustomRenderers
{
    public class ButtonView : View
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create<RoundedBoxView, double>(p => p.CornerRadius, 0);
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create<RoundedBoxView, Color>(p => p.Stroke, Color.Red);
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        //public static readonly BindableProperty StrokeProperty = BindableProperty.Create<RoundedBoxView, Color>(p => p.Stroke, Color.Transparent);
        //public Color Stroke
        //{
        //    get { return (Color)GetValue(StrokeProperty); }
        //    set { SetValue(StrokeProperty, value); }
        //}

        public static readonly BindableProperty StrokeThickProperty = BindableProperty.Create<RoundedBoxView, double>(p => p.StrokeThick, 2);

        public double StrokeThick
        {
            get { return (double)GetValue(StrokeThickProperty); }
            set { SetValue(StrokeThickProperty, value); }
        }
    }
}
