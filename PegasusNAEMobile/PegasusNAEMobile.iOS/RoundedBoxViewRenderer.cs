using System;
using System.Collections.Generic;
using System.Text;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using UIKit;

namespace PegasusNAEMobile.iOS
{
    public class RoundedBoxViewRenderer : BoxRenderer
    {

        public override void Draw(CGRect rect)
        {
            RoundedBoxView rbv = (RoundedBoxView)this.Element;
            //using (var context = UIGraphics.GetCurrentContext())
            //{
            //    context.SetFillColor
            //}
            base.Draw(rect);
        }
    }
}
