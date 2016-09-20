﻿using PegasusNAEMobile;
using PegasusNAEMobile.WinPhone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.WinRT;

[assembly: ExportRendererAttribute(typeof(VideoPlayerView), typeof(VideoPlayerViewRenderer))]
namespace PegasusNAEMobile.WinPhone
{
    public class VideoPlayerViewRenderer : ViewRenderer<VideoPlayerView, MediaElement>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayerView> e)
        {
            base.OnElementChanged(e);
            var playerview = Element as VideoPlayerView;
            if (e.OldElement == null && playerview != null)
            {
                MediaElement meplayer = new MediaElement();

                meplayer.AutoPlay = true;
                meplayer.Source = new Uri(playerview.FileSource);
                meplayer.IsFullWindow = true;
                // meplayer.
                meplayer.AreTransportControlsEnabled = true;
                Children.Add(meplayer);
            }
        }
    }
}