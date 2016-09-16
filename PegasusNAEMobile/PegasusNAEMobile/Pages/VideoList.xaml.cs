using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PegasusNAEMobile.Collections;

using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public partial class VideoList : ContentPage
    {
        private double width = 0;
        private double height = 0;
        private PreviousRunCollection runcollect { get; set; }
        public VideoList(PreviousRunCollection runcollect)
        {
            InitializeComponent();
            MainGrid.BackgroundColor = Color.Black;
            this.runcollect = runcollect;
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            if (Device.OS == TargetPlatform.iOS)
            {
                BackButton.Image = "back.png";
                //TakeToVideosPage.Image = "videocam.png";
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                BackButton.Image = "back.png";
                BackButton.BackgroundColor = Color.Transparent;
                //TakeToVideosPage.Image = "videocam.png";
            }
            else
            {
                //BackButton.Image = "Assets/" + "Back.png";
                //TakeToVideosPage.Image = "Assets/videocam.png";
                BackButton.IsVisible = false;
                BackButton.IsEnabled = false;
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
                Drone1VideoFrame.IsVisible = true;
                Drone2VideoFrame.IsVisible = true;
                OnboardVideoFrame.IsVisible = true;
                Drone1VideoButton.Image = "Assets/" + "videocam.png";
                Drone1VideoButton.BackgroundColor = Color.Transparent;
                Drone2VideoButton.BackgroundColor = Color.Transparent;
                OnboardVideoButton.BackgroundColor = Color.Transparent;
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

        private async void Drone1VideoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowVideo(runcollect.Drone1VideoUrl));
        }

        private async void Drone2VideoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowVideo(runcollect.Drone2VideoUrl));
        }
        private async void OnboardVideoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShowVideo(runcollect.OnboardVideoUrl));
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
