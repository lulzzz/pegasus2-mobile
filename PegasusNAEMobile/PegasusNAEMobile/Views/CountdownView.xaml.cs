using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public partial class CountdownView : ContentView
    {
        public CountdownView()
        {
            InitializeComponent();
            // asgangal : I was going to put this in CountdownViewModel and bind it as an Image Source, but I was doing something wrong there because the app kept crashing. We should fix this.
            pegasus_HeroBackground.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("pegasus_herobackground.png"),
            Android: ImageSource.FromFile("pegasus_herobackground.png"),
            WinPhone: ImageSource.FromFile("Assets/pegasus_herobackground.png"));
        }

        
    }
}
