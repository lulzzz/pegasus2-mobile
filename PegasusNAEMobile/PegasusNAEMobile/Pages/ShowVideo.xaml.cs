using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public partial class ShowVideo : ContentPage
    {
        public ShowVideo()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 0);
            if (Device.OS == TargetPlatform.iOS)
            {
                BackButton.Image = "Back.png";
                //TakeToVideosPage.Image = "videocam.png";
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                BackButton.Image = "Back.png";
                //TakeToVideosPage.Image = "videocam.png";
            }
            else
            {
                BackButton.Image = "Assets/" + "Back.png";
                //TakeToVideosPage.Image = "Assets/videocam.png";
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}
