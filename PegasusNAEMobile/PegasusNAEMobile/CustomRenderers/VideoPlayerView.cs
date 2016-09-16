using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public class VideoPlayerView : View
    {
        public static readonly BindableProperty FileSourceProperty = BindableProperty.Create<VideoPlayerView, string>( p => p.FileSource, string.Empty);

        public string FileSource
        {
            get { return (string)GetValue(FileSourceProperty); }
            set
            {
                SetValue(FileSourceProperty, value);
            }
        }
    }
}
