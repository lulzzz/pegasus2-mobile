using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public partial class CountdownInformationPage : ContentPage
    {
        public CountdownInformationPage()
        {
            InitializeComponent();
            this.BindingContext = new CountdownInformationViewModel();
        }
    }
}
