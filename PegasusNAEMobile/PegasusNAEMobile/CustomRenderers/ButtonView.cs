using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public class ButtonView : View
    {
        public static readonly BindableProperty IconProperty = BindableProperty.Create<ButtonView, string>(p => p.Icon, string.Empty);
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create<ButtonView, Color>(p => p.BorderColor, Color.Transparent);
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }        
    }
}
