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
        public ShowVideo(string video_url)
        {
            InitializeComponent();
            VideoElement.Source = video_url;
            NavigationPage.SetHasNavigationBar(this, false);
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            if (Device.OS == TargetPlatform.iOS)
            {
                BackButton.Image = "Back.png";
                //TakeToVideosPage.Image = "videocam.png";
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                BackButton.Image = "back.png";
                //TakeToVideosPage.Image = "videocam.png";
            }
            else
            {
                BackButton.Image = "Assets/" + "Back.png";
                //TakeToVideosPage.Image = "Assets/videocam.png";
            }
            this.SizeChanged += ShowVideo_SizeChanged;
            
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
