using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PegasusData;
using Xamarin.Forms;
using PegasusNAEMobile.Collections;
using System.Threading;
using Newtonsoft.Json;

namespace PegasusNAEMobile
{
    public partial class NonLiveEventTelemetry : ContentPage
    {
        private double height = 0;
        private double width = 0;
        private RootObjectOnboardTelemetry ronboardtelem;
        private PreviousRunCollection runcollect;
        private int currenttelemetrypos;   // used to keep track of what telemetry has been displayed and what's left
        private double  prevslidervalue = -1;
        
        CancellationTokenSource cancellation;  // Used to check if Timer cancellation has been requested.
        private bool PlayPauseIcon; // TRUE - Display Pause Icon , FALSE - Display Play Icon  
        public NonLiveEventTelemetry(PreviousRunCollection rpc)
        {
            InitializeComponent();
            MainGrid.BackgroundColor = Color.Black;
            NavigationPage.SetHasNavigationBar(this, false);    // Hides the navigation bar.
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            runcollect = new PreviousRunCollection();
            runcollect = rpc;
            PlayPauseIcon = true; //Pause Icon is default
            ronboardtelem = new RootObjectOnboardTelemetry();
            currenttelemetrypos = 0;
            cancellation = new CancellationTokenSource();      // This will be used to stop the timer when the user leaves the page.
            updateButtonIcon("pause.png");

            // Add the Vehicle Image to the view.
            vehicle.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("pegasus_vehicle_small.png"),
            Android: ImageSource.FromFile("pegasus_vehicle_small.png"),
            WinPhone: ImageSource.FromFile("Assets/pegasus_vehicle_small.png"));
            //TelemetrySlider.PropertyChanged += TelemetrySlider_PropertyChanged;
            TelemetrySlider.ValueChanged += TelemetrySlider_ValueChanged;
            TelemetrySlider.BindingContext = currenttelemetrypos;
            if (Device.OS == TargetPlatform.iOS)
            {
                BackButton.Image = "back.png";
                TakeToVideosPage.Image = "videocam.png";
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                BackButton.Image = "back.png";
                BackButton.BackgroundColor = Color.Transparent;
                TakeToVideosPage.Image = "videocam.png";
                TakeToVideosPage.BackgroundColor = Color.Transparent;
            }
            else
            {
                //BackButton.Image = "Assets/" + "Back.png";
                TakeToVideosPage.Image = "Assets/videocam.png";
                BackButton.IsVisible = false;
                BackButton.IsEnabled = false;
            }
           // System.Diagnostics.Debug.WriteLine(Constants.ScreenHeight + ", " + Constants.ScreenWidth);    
        }

        private void TelemetrySlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            System.Diagnostics.Debug.WriteLine("Previous Slider Value : " + prevslidervalue + "  Current Slider Value : " + slider.Value);
            if (prevslidervalue > Math.Ceiling(slider.Value))
            {
                System.Diagnostics.Debug.WriteLine("User moved the slider");
                slider.Value = Math.Ceiling(slider.Value);
                cancellation.Cancel();
                currenttelemetrypos = (int)Math.Ceiling(slider.Value);
            }
            prevslidervalue = Math.Ceiling(slider.Value);
            
            //throw new NotImplementedException();
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
                
                double fontsizelarge = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                double fontsizemedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                double fontsizemicro = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
                double fontsizesmall = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                RunTimeStampNonLive.FontSize = fontsizesmall;
                RunIDNonLive.FontSize = fontsizesmall;
                SpeedGridTitleNonLive.FontSize = fontsizesmall;
                SpeedKPHNonLive.FontSize = fontsizelarge;
                kph.FontSize = fontsizemicro;
                StickPositionLabel.FontSize = fontsizesmall;
                StickPosition.FontSize = fontsizelarge;
                ThrottlePositionLabel.FontSize = fontsizesmall;
                ThrottlePosition.FontSize = fontsizelarge;
                SteeringBoxLabel.FontSize = fontsizesmall;
                SteeringBoxPositionNonLive.FontSize = fontsizelarge;
                degrees.FontSize = fontsizesmall;
                MaxAcclLabel.FontSize = fontsizesmall;
                RearLeftLabel.FontSize = fontsizesmall;
                RearRightLabel.FontSize = fontsizesmall;
                MaxAcclValue.FontSize = fontsizesmall;
                NoseWeightLabel.FontSize = fontsizesmall;
                SideToSideLabel.FontSize = fontsizesmall;
                NoseWeightNonLive.FontSize = fontsizesmall;
                SideToSide.FontSize = fontsizesmall;
                AccelX_Label.FontSize = fontsizemicro;
                AccelY_Label.FontSize = fontsizemicro;
                AccelZ_Label.FontSize = fontsizemicro;
                if (this.width < 370)
                {
                    AccelX.FontSize = fontsizesmall;
                    AccelY.FontSize = fontsizesmall;
                    AccelZ.FontSize = fontsizesmall;
                    gLabelX.FontSize = fontsizemicro;
                    gLabelY.FontSize = fontsizemicro;
                    gLabelZ.FontSize = fontsizemicro;
                    RearLeft.FontSize = fontsizesmall;
                    RearRight.FontSize = fontsizesmall;
                }
                else if (this.width < 330)
                {
                    AccelX.FontSize = fontsizemicro;
                    AccelY.FontSize = fontsizemicro;
                    AccelZ.FontSize = fontsizemicro;
                    gLabelX.FontSize = fontsizemicro;
                    gLabelY.FontSize = fontsizemicro;
                    gLabelZ.FontSize = fontsizemicro;
                    RearLeft.FontSize = fontsizesmall;
                    RearRight.FontSize = fontsizesmall;
                }
                else
                {
                    RearLeft.FontSize = fontsizesmall;
                    RearRight.FontSize = fontsizesmall;
                    AccelX.FontSize = fontsizesmall;
                    AccelY.FontSize = fontsizesmall;
                    AccelZ.FontSize = fontsizesmall;
                }
                SteeringAcclLabel.FontSize = fontsizesmall;
                NoFileLabel.FontSize = fontsizemedium;
                RegisterForEventNotifications.FontSize = fontsizesmall;
            }
        }

        protected async override void OnAppearing()
        {
            try
            {
                string onboardtelemetry = await App.Instance.GetFileFromBlob(runcollect.OnboardTelemetryUrl);   // Get the telemetry JSON file from the blob storage
                if (String.IsNullOrEmpty(onboardtelemetry))   // File was empty or there was a problem getting the file
                {
                    NoFileAvailable.IsVisible = true;        // Show message that data isn't ready yet.
                    FileAvailable.IsVisible = false;
                }
                else
                {
                    NoFileAvailable.IsVisible = false;
                    FileAvailable.IsVisible = true;
                    List<Collection> telemetrylist = new List<Collection>();
                    telemetrylist = JsonConvert.DeserializeObject<List<Collection>>(onboardtelemetry);
                    ronboardtelem.collection = new List<Collection>();
                    foreach (var telemetry in telemetrylist)
                    {
                        ronboardtelem.collection.Add(telemetry);
                    }
                   // ronboardtelem = OnboardTelemetryCollection.DataDeserializer(onboardtelemetry);
                    TelemetrySlider.Minimum = 0;
                    TelemetrySlider.Maximum = ronboardtelem.collection.Count;
                    TimeSpan duration = new TimeSpan(0, 0, 0, 0, (ronboardtelem.collection.Count * 500));
                    TotalTelemetryPlaybackTime.Text = duration.ToString("g");
                    TimeSpan T = TimeSpan.FromSeconds(0.5);
                    UpdateNonLiveTelemetryUI(T);
                }
            }
            catch (Exception ex)
            {
                NoFileAvailable.IsVisible = true;
                FileAvailable.IsVisible = false;
            }            
            //System.Diagnostics.Debug.WriteLine(rtelem.collection.Count);
            base.OnAppearing();
        }

        private void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            if (PlayPauseIcon)   // Pause is displayed. So Pause the playback
            {
                updateButtonIcon("play.png");
                System.Diagnostics.Debug.WriteLine("Previous Slider Value : " + prevslidervalue + "  Current Slider Value : " + TelemetrySlider.Value);

                cancellation.Cancel();
                PlayPauseIcon = false;   // Because the button has been changed to show the play icon
            }
            else   // Play icon is displayed. Set it to Pause and start the playback.
            {
                updateButtonIcon("pause.png");
                PlayPauseIcon = true;
                prevslidervalue = Math.Ceiling(TelemetrySlider.Value);
                cancellation = new CancellationTokenSource();
                TimeSpan T = TimeSpan.FromSeconds(0.5);
                UpdateNonLiveTelemetryUI(T);
            }
        }

        protected override void OnDisappearing()
        {
            //Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
            currenttelemetrypos = 0;
            TelemetrySlider.Value = 0;
            cancellation.Cancel();
            base.OnDisappearing();
        }

        /// <summary>
        /// Callback method for the Timer. Updates the UI every T seconds.
        /// </summary>
        private void UpdateNonLiveTelemetryUI(TimeSpan T)
        {
            Device.StartTimer(T, () =>
            {
                CancellationTokenSource cts = cancellation;
                if (cts.IsCancellationRequested)
                {
                    System.Diagnostics.Debug.WriteLine("Timer cancellation requested");
                    
                    updateButtonIcon("play.png");
                    PlayPauseIcon = false;
                    return false;
                }
                if (currenttelemetrypos < ronboardtelem.collection.Count)
                {
                    System.Diagnostics.Debug.WriteLine(currenttelemetrypos);
                    var currenttelemetry = ronboardtelem.collection[currenttelemetrypos];
                    RunTimeStampNonLive.Text = String.Format("{0:MMMM dd, yyyy}", currenttelemetry.timestamp);
                    
                    SpeedKPHNonLive.Text = RoundToDecimalPlaces(currenttelemetry.AirSpeedKph);
                    SteeringBoxPositionNonLive.Text = RoundToDecimalPlaces(currenttelemetry.SteeringBoxPositionDegrees);
                    NoseWeightNonLive.Text = RoundToDecimalPlaces(currenttelemetry.NoseWeightLbf) + " lbf";
                    //SideToSide.Text = (currenttelemetry.)
                    ThrottlePosition.Text = RoundToDecimalPlaces(currenttelemetry.ThrottlePosition);
                    StickPosition.Text = RoundToDecimalPlaces(currenttelemetry.StickPosition);
                    RearLeft.Text = RoundToDecimalPlaces(currenttelemetry.LeftRearWeightLbf) + " lbf";
                    RearRight.Text = RoundToDecimalPlaces(currenttelemetry.RightRearWeightLbf) + " lbf";
                    TelemetrySlider.Value = currenttelemetrypos;
                    AccelX.Text = RoundToDecimalPlaces(currenttelemetry.SteerBoxAccelXG);
                    AccelY.Text = RoundToDecimalPlaces(currenttelemetry.SteerBoxAccelYG);
                    AccelZ.Text = RoundToDecimalPlaces(currenttelemetry.SteerBoxAccelZG);
                    MaxAcclValue.Text = RoundToDecimalPlaces(currenttelemetry.AccelXG) + "g";
                    SideToSide.Text = RoundToDecimalPlaces(currenttelemetry.AccelYG) + "g";
                    //MaxAccl.Text = (currenttelemetry.acc)
                    currenttelemetrypos++;
                    return true;                    
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Stopping Timer");
                    currenttelemetrypos = 0;
                    TelemetrySlider.Value = 0;
                    updateButtonIcon("play.png");
                    PlayPauseIcon = false;
                    return false;
                }
            });
            
        }

        private void RegisterForEventNotifications_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://www.pegasusmission.io/Home/Notifications");
            Device.OpenUri(uri);
        }

        private void updateButtonIcon(string iconfilename)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                PlayPauseButton.Image = iconfilename;
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                PlayPauseButton.Image = iconfilename;
            }
            else
            {
                PlayPauseButton.Image = "Assets/" + iconfilename;
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void TakeToVideosPage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VideoList(runcollect));
        }

        private string RoundToDecimalPlaces(double val)
        {
            double round = (Math.Round((double)val, 2 , MidpointRounding.AwayFromZero));
            return (String.Format("{0:0.00}", round));
        }
    }
}
