using System;
using System.Collections.Generic;
using System.Text;
using CoreGraphics;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using PegasusNAEMobile;
using PegasusNAEMobile.iOS;
using System.ComponentModel;

[assembly : ExportRendererAttribute(typeof(RoundedBoxView), typeof(RoundedBoxViewRenderer))]
namespace PegasusNAEMobile.iOS
{
    public class RoundedBoxViewRenderer : VisualElementRenderer<RoundedBoxView>
    {

        public override void Draw(CGRect rect)
        {
            RoundedBoxView rbv = (RoundedBoxView)this.Element;
            using (var context = UIGraphics.GetCurrentContext())
            {
                var path = CGPath.EllipseFromRect(rect);
                context.AddPath(path);
                Color strokecolor = Color.FromHex("#d90000");
                context.SetStrokeColor(strokecolor.ToCGColor());
                context.SetLineWidth(3);
                context.DrawPath(CGPathDrawingMode.Stroke);
                //context.SetFillColor(rbv.Color.ToCGColor());
                //context.SetStrokeColor(rbv.Stroke.ToCGColor());
                //context.SetLineWidth((float)rbv.StrokeThick);
                //var rc = this.Bounds.Inset((int)rbv.StrokeThick, (int)rbv.StrokeThick);
                //float radius = (float)rbv.CornerRadius;
                //radius = (float)Math.Max(0, Math.Min(radius, Math.Max(rc.Height / 2, rc.Width / 2)));
                //var path = CGPath.FromRoundedRect(rc, radius, radius);
                //context.AddPath(path);
                //context.DrawPath(CGPathDrawingMode.FillStroke);
            }
            //base.Draw(rect);
        }       
    }
}
