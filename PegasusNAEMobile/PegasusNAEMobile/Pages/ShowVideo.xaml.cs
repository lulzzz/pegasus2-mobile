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
        private double width = 0;
        private double height = 0;
        private bool videostarted = false;
        public ShowVideo(string video_url)
        {
            InitializeComponent();
            MainGrid.BackgroundColor = Color.Black;
            VideoElement.Source = video_url;
            VideoElement.Failed += VideoElement_Failed;
            VideoElement.Playing += VideoElement_Playing;
            VideoElement.Paused += VideoElement_Paused;
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
                ActivityIndicate.HorizontalOptions = LayoutOptions.Center;
                ActivityIndicate.VerticalOptions = LayoutOptions.Center;
                VideoElement.HorizontalOptions = LayoutOptions.CenterAndExpand;
                VideoElement.VerticalOptions = LayoutOptions.CenterAndExpand;
                //TakeToVideosPage.Image = "videocam.png";
            }
            else
            {
                //BackButton.Image = "Assets/" + "Back.png";
                //TakeToVideosPage.Image = "Assets/videocam.png";
                BackButton.IsVisible = false;
                BackButton.IsEnabled = false;
            }
            this.SizeChanged += ShowVideo_SizeChanged;
            
        }

        protected override void OnAppearing()
        {
            if (videostarted == false)
            {
                ActivityIndicate.IsVisible = true;
                ActivityIndicate.IsRunning = true;
            }
            base.OnAppearing();
        }

        private void VideoElement_Paused(object sender, Octane.Xam.VideoPlayer.Events.VideoPlayerEventArgs e)
        {
            //throw new NotImplementedException();
            ActivityIndicate.IsVisible = false;
            ActivityIndicate.IsRunning = false;
            System.Diagnostics.Debug.WriteLine("Paused");
        }

        private void VideoElement_Playing(object sender, Octane.Xam.VideoPlayer.Events.VideoPlayerEventArgs e)
        {
            //throw new NotImplementedException();
            videostarted = true;
            ActivityIndicate.IsVisible = false;
            ActivityIndicate.IsRunning = false;
            System.Diagnostics.Debug.WriteLine("Playing");
        }

        private void VideoElement_Failed(object sender, Octane.Xam.VideoPlayer.Events.VideoPlayerErrorEventArgs e)
        {
            //throw new NotImplementedException();
            System.Diagnostics.Debug.WriteLine("Failed");
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

        private void ShowVideo_SizeChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}
