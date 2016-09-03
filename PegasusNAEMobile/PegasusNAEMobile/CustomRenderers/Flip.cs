using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PegasusNAEMobile.CustomRenderers
{
    public class Flip : View
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create<Flip, IEnumerable>(p => p.ItemsSource, null, propertyChanged: ItemsSourceChanged);
        public static readonly BindableProperty OrientationProperty = BindableProperty.Create<Flip, ScrollOrientation>(p => p.Orientation, ScrollOrientation.Horizontal);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create<Flip, DataTemplate>(p => p.ItemTemplate, null);
        public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create<Flip, bool>(p => p.AutoPlay, false, propertyChanged: AutoPlayChanged);
        public static readonly BindableProperty IntervalProperty = BindableProperty.Create<Flip, int>(p => p.Interval, 2000);

        public event EventHandler NextRequired;
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }

        public ScrollOrientation Orientation
        {
            get
            {
                return (ScrollOrientation)this.GetValue(OrientationProperty);
            }
            set
            {
                this.SetValue(OrientationProperty, value);
            }
        }

        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ItemTemplateProperty);
            }
            set
            {
                this.SetValue(ItemTemplateProperty, value);
            }
        }

        public bool AutoPlay
        {
            get
            {
                return (bool)this.GetValue(AutoPlayProperty);
            }
            set
            {
                this.SetValue(AutoPlayProperty, value);
            }

        }

        public int Interval
        {
            get
            {
                return (int)this.GetValue(IntervalProperty);
            }
            set
            {
                this.SetValue(IntervalProperty, value);
            }
        }
        
        public IEnumerable<View> Children
        {
            get;
            private set;
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var flip = (Flip)bindable;
            flip.CalcChild();
        }

        private static void AutoPlayChanged(BindableObject bindable, bool oldValue, bool newValue)
        {
            var flip = (Flip)bindable;
            if (newValue)
            {
                flip.Play();
            }
            else
            {
                flip.Stop();
            }
        }
        private void CalcChild()
        {
            var children = new List<View>();
            if (this.ItemsSource == null || this.ItemTemplate == null)
            {
                return;
            }
            foreach (var o in this.ItemsSource)
            {
                var view = (View)this.ItemTemplate.CreateContent();
                view.BindingContext = o;
                children.Add(view);
                view.Parent = this;
            }
            this.Children = children;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            foreach (var c in this.Children)
            {
                c.Layout(new Rectangle(0, 0, width, height));
            }
        }

        public void Play()
        {
            this.AutoPlay = true;
            this.InnerPlay();
        }

        public void Stop()
        {
            this.AutoPlay = false;
        }

        private void InnerPlay()
        {
            if (this.AutoPlay)            
                Task.Delay(this.Interval).ContinueWith(t =>
                {   if (this.NextRequired != null)
                    {
                        this.NextRequired.Invoke(this, new EventArgs());
                    }
                    this.InnerPlay();});

        }
    }
}
