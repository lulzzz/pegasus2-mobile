using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PegasusNAEMobile.Pages
{
    public partial class About : ContentPage
    {
        private double height = 0;
        private double width = 0;
        public About()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);    // Hides the navigation bar.
            if (Device.OS == TargetPlatform.iOS)
            {
                BackButton.Image = "Back.png";
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                BackButton.Image = "back.png";
                BackButton.BackgroundColor = Color.Transparent;                
            }
            else
            {
                // BackButton.Image = "Assets/" + "Back.png";
                BackButton.IsVisible = false;
                BackButton.IsEnabled = false;
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {

            base.OnSizeAllocated(width, height);
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;

                if (width > height)
                {
                    Padding = new Thickness(0, 0, 0, 0);

                }
                else
                {
                    Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
                }
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void landspeedlink_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://www.landspeed.com/");
            Device.OpenUri(uri);
        }

        private void LandspeedTwitterLink_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://twitter.com/landspeed763");
            Device.OpenUri(uri);
        }

        private void PegasusMissionTwitterLink_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://twitter.com/PegasusMission");
            Device.OpenUri(uri);
        }
    }
}
