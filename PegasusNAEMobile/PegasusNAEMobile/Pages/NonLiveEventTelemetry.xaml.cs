﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PegasusData;
using Xamarin.Forms;
using PegasusNAEMobile.Collections;
using System.Threading;

namespace PegasusNAEMobile
{
    public partial class NonLiveEventTelemetry : ContentPage
    {
        private RootObjectOnboardTelemetry ronboardtelem;
        private PreviousRunCollection runcollect;
        private int currenttelemetrypos;   // used to keep track of what telemetry has been displayed and what's left
        CancellationTokenSource cancellation;  // Used to check if Timer cancellation has been requested.
        private bool PlayPauseIcon; // TRUE - Display Pause Icon , FALSE - Display Play Icon  
        public NonLiveEventTelemetry(PreviousRunCollection rpc)
        {
            InitializeComponent();
            runcollect = new PreviousRunCollection();
            runcollect = rpc;
            PlayPauseIcon = true; //Pause Icon is default
            ronboardtelem = new RootObjectOnboardTelemetry();
            currenttelemetrypos = 0;
            cancellation = new CancellationTokenSource();
            updateButtonIcon("Pause.png");
            vehicle.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("pegasus_vehicle_small.png"),
            Android: ImageSource.FromFile("pegasus_vehicle_small.png"),
            WinPhone: ImageSource.FromFile("Assets/pegasus_vehicle_small.png"));
            TelemetrySlider.PropertyChanged += TelemetrySlider_PropertyChanged;
            TelemetrySlider.ValueChanged += TelemetrySlider_ValueChanged;
            TelemetrySlider.BindingContext = currenttelemetrypos;
            
            System.Diagnostics.Debug.WriteLine(Constants.ScreenHeight + ", " + Constants.ScreenWidth);    
        }

        //public NonLiveEventTelemetry(PreviousRunCollection rprcollect)
        //{
        //   // runcollect = new PreviousRunCollection();
        //    this.runcollect = rprcollect;
        //   // InitializeComponent();
        //}
        private void TelemetrySlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            //throw new NotImplementedException();
            //System.Diagnostics.Debug.WriteLine("Value Changed");
        }

        private void TelemetrySlider_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //throw new NotImplementedException();
            //System.Diagnostics.Debug.WriteLine("Property Changed : " + e.PropertyName);
        }

        protected async override void OnAppearing()
        {
            string onboardtelemetry = await App.Instance.GetFileFromBlob(runcollect.OnboardTelemetryUrl);
            ronboardtelem = OnboardTelemetryCollection.DataDeserializer(onboardtelemetry);
            TelemetrySlider.Minimum = 0;
            TelemetrySlider.Maximum = ronboardtelem.collection.Count;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, (ronboardtelem.collection.Count * 500));
            TotalTelemetryPlaybackTime.Text = duration.ToString("g");
            TimeSpan T = TimeSpan.FromSeconds(0.5);
            UpdateNonLiveTelemetryUI(T);
            //System.Diagnostics.Debug.WriteLine(rtelem.collection.Count);
            base.OnAppearing();
        }

        private void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            if (PlayPauseIcon)   // Pause is displayed. So Pause the playback
            {
                updateButtonIcon("Play.png");
                cancellation.Cancel();
                PlayPauseIcon = false;   // Because the button has been changed to show the play icon
            }
            else   // Play icon is displayed. Set it to Pause and start the playback.
            {
                updateButtonIcon("Pause.png");
                PlayPauseIcon = true;
                cancellation = new CancellationTokenSource();
                TimeSpan T = TimeSpan.FromSeconds(0.5);
                UpdateNonLiveTelemetryUI(T);
            }
        }
        protected override void OnDisappearing()
        {
            //Interlocked.Exchange(ref this.cancellation, new CancellationTokenSource()).Cancel();
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
                    return false;
                }
                if (currenttelemetrypos < ronboardtelem.collection.Count)
                {
                    System.Diagnostics.Debug.WriteLine(currenttelemetrypos);
                    var currenttelemetry = ronboardtelem.collection[currenttelemetrypos];
                    RunTimeStampNonLive.Text = String.Format("{0:MMMM dd, yyyy}", currenttelemetry.timestamp);
                    SpeedKPHNonLive.Text = RoundToDecimalPlaces(currenttelemetry.AirSpeedKph);
                    SteeringBoxPositionNonLive.Text = RoundToDecimalPlaces(currenttelemetry.SteeringBoxPositionDegrees);
                    NoseWeightNonLive.Text = RoundToDecimalPlaces(currenttelemetry.NoseWeightLbf);
                    //SideToSide.Text = (currenttelemetry.)
                    RearLeft.Text = RoundToDecimalPlaces(currenttelemetry.LeftRearWeightLbf);
                    RearRight.Text = RoundToDecimalPlaces(currenttelemetry.RightRearWeightLbf);
                    TelemetrySlider.Value = currenttelemetrypos;
                    
                    //MaxAccl.Text = (currenttelemetry.acc)
                    currenttelemetrypos++;
                    return true;                    
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Stopping Timer");
                    currenttelemetrypos = 0;
                    TelemetrySlider.Value = 0;
                    updateButtonIcon("Play.png");
                    PlayPauseIcon = false;
                    return false;
                }
            });
            
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

        private string RoundToDecimalPlaces(double val)
        {
            double round = (Math.Round((double)val, 2 , MidpointRounding.AwayFromZero));
            return (String.Format("{0:0.00s}", round));
        }
    }
}
