using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PegasusNAEMobile;
using PegasusNAEMobile.Droid;
using Android.Graphics;

[assembly: ExportRendererAttribute(typeof(RoundedBoxView), typeof(RoundedBoxViewRenderer))]
namespace PegasusNAEMobile.Droid
{
    public class RoundedBoxViewRenderer : VisualElementRenderer<RoundedBoxView>
    {

        public RoundedBoxViewRenderer()
        {
            this.SetWillNotDraw(false);
        }
       
        public override void Draw(Canvas canvas)
        {
            try
            {
                var element = this.Element;
                //var rect = new Rect();
                //this.GetDrawingRect(rect);
                var paint = new Paint()
                {
                    Color = (Xamarin.Forms.Color.FromHex("#d90000")).ToAndroid(),
                    AntiAlias = true,
                    StrokeWidth = 3
                };
                paint.SetStyle(Paint.Style.Stroke);
                
                canvas.DrawCircle(this.Width/2, this.Height/2, this.Height/2, paint);
               
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
            }
            
        }
    }
}