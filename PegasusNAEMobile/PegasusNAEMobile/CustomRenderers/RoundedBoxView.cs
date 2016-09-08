using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PegasusNAEMobile
{ 
    public class RoundedBoxView : BoxView
    {
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create<RoundedBoxView, double>(p => p.CornerRadius, 0);
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty StrokeProperty = BindableProperty.Create<RoundedBoxView, Color>(p => p.Stroke, Color.Transparent);
        public Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly BindableProperty StrokeThickProperty = BindableProperty.Create<RoundedBoxView, double>(p => p.StrokeThick, default(double));

        public double StrokeThick
        {
            get { return (double)GetValue(StrokeThickProperty); }
            set { SetValue(StrokeThickProperty, value); }
        }
    }
}
