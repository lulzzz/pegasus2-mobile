using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public partial class VideoList : ContentPage
    {
        public VideoList()
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
            if (Device.OS == TargetPlatform.iOS)
            {
                Drone1VideoButton.Image = "videocam.png";
                Drone2VideoButton.Image = "videocam.png";
                OnboardVideoButton.Image = "videocam.png";
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                Drone1VideoButton.Image = "videocam.png";
                Drone2VideoButton.Image = "videocam.png";
                OnboardVideoButton.Image = "videocam.png";
            }
            else
            {
                Drone1VideoButton.Image = "Assets/" + "videocam.png";
                Drone2VideoButton.Image = "Assets/" + "videocam.png";
                OnboardVideoButton.Image = "Assets/" + "videocam.png";
            }
            Drone1VideoFrame.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("NAE_ScaledDown.png"),
            Android: ImageSource.FromFile("NAE_ScaledDown.png"),
            WinPhone: ImageSource.FromFile("Assets/NAE_ScaledDown.png"));

            Drone2VideoFrame.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("NAE_ScaledDown.png"),
            Android: ImageSource.FromFile("NAE_ScaledDown.png"),
            WinPhone: ImageSource.FromFile("Assets/NAE_ScaledDown.png"));

            OnboardVideoFrame.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("NAE_ScaledDown.png"),
            Android: ImageSource.FromFile("NAE_ScaledDown.png"),
            WinPhone: ImageSource.FromFile("Assets/NAE_ScaledDown.png"));
        }

        private async void Drone1VideoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowVideo());
        }

        private async void Drone2VideoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowVideo());
        }
        private async void OnboardVideoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowVideo());
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
