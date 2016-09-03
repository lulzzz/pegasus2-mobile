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
    }
}
