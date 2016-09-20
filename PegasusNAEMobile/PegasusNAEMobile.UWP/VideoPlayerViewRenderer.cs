using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.UWP;
using PegasusNAEMobile;
using PegasusNAEMobile.UWP;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Windows.Media.Playback;

[assembly: ExportRendererAttribute(typeof(VideoPlayerView), typeof(VideoPlayerViewRenderer))]
namespace PegasusNAEMobile.UWP
{
    public class VideoPlayerViewRenderer : ViewRenderer<VideoPlayerView, MediaElement>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayerView> e)
        {
            base.OnElementChanged(e);
            //MediaPlayerElement 
             
            var playerview = Element as VideoPlayerView;
            if (e.OldElement == null && playerview != null)
            {
                MediaElement meplayer = new MediaElement();
                
                meplayer.AutoPlay = true;
                meplayer.Source = new Uri(playerview.FileSource);
                meplayer.IsFullWindow = true;
               // meplayer.
                meplayer.AreTransportControlsEnabled = true;
                meplayer.Volume = 0.5;
                meplayer.MediaEnded += Meplayer_MediaEnded;
                Children.Add(meplayer);
            }
        }

        private void Meplayer_MediaEnded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            var meplayer = sender as MediaElement;
            
           // meplayer.
        }
    }
}
