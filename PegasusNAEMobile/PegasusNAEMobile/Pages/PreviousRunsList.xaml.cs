using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PegasusNAEMobile.Collections;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace PegasusNAEMobile
{
    public partial class PreviousRunsList : ContentPage
    {
        private double width = 0;
        private double height = 0;
        public ObservableCollection<PreviousRunCollection> runlist { get; set; }
        public PreviousRunsList()
        {
            runlist = new ObservableCollection<PreviousRunCollection>();
            InitializeComponent();
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            NavigationPage.SetHasNavigationBar(this, false);    // Hides the navigation bar.
            if (Device.OS == TargetPlatform.iOS)
            {
                BackButton.Image = "Back.png";
            }
            else if (Device.OS == TargetPlatform.Android)
            {
                BackButton.Image = "back.png";
                ActivityIndicate.HorizontalOptions = LayoutOptions.Center;
                ActivityIndicate.VerticalOptions = LayoutOptions.Center;
            }
            else
            {
                BackButton.Image = "Assets/" + "Back.png";
            }
        }

        private void RegisterForEventNotifications_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://www.pegasusmission.io/Home/Notifications");
            Device.OpenUri(uri);
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

        protected async override void OnAppearing()
        {
            ActivityIndicate.IsVisible = true;
            ActivityIndicate.IsRunning = true;
            try
            {
                string configjson = await App.Instance.GetFileFromBlob("https://pegasustest.blob.core.windows.net/pegasustestblob/config.json");
                if (String.IsNullOrEmpty(configjson))
                {
                    NoFileAvailable.IsVisible = true;
                    FileAvailable.IsVisible = false;
                }
                else
                {
                    NoFileAvailable.IsVisible = false;
                    FileAvailable.IsVisible = true;
                    List<CollectionConfig> configList = new List<CollectionConfig>();
                    configList = JsonConvert.DeserializeObject<List<CollectionConfig>>(configjson);
                   
                    runlist.Clear();
                    foreach (var config in configList)
                    {
                        PreviousRunCollection runcollect = new PreviousRunCollection();                       
                        runcollect.Drone1VideoUrl = config.Drone1VideoUrl;
                        runcollect.Drone2VideoUrl = config.Drone2VideoUrl;
                        runcollect.Location = config.Location;
                        runcollect.OnboardTelemetryUrl = config.OnboardTelemetryUrl;
                        runcollect.OnboardVideoUrl = config.OnboardVideoUrl;
                        runcollect.Pilot = config.Pilot;
                        runcollect.RunId = config.RunId;
                        runcollect.Timestamp = config.Timestamp;
                        runcollect.maxAccelX = config.Aggregates.MaxAccelX;
                        runcollect.maxAccelY = config.Aggregates.MaxAccelY;
                        runcollect.maxAccelZ = config.Aggregates.MaxAccelZ;
                        runcollect.maxSpeed = config.Aggregates.MaxSpeed;
                        addRunToUI(runcollect);
                        runlist.Add(runcollect);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                NoFileAvailable.IsVisible = true;
                FileAvailable.IsVisible = false;
            }
            ActivityIndicate.IsRunning = false;
            ActivityIndicate.IsVisible = false;
            //PreviousRunListView.ItemsSource = runlist;
            base.OnAppearing();
        }

        private void addRunToUI(PreviousRunCollection runcollect)
        {
            double fontsizemedium = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            double fontsizelarge = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            Grid childGrid = new Grid{
                RowDefinitions =
                {
                    new RowDefinition {Height = GridLength.Auto },
                    new RowDefinition {Height = GridLength.Auto },
                    new RowDefinition {Height = GridLength.Auto }
                }                   
            };
            
            BoxView bview = new BoxView();
            bview.Color = Color.FromHex("#d90000");
            bview.HorizontalOptions = LayoutOptions.FillAndExpand;
            bview.VerticalOptions = LayoutOptions.Start;
            bview.HeightRequest = 1;
            childGrid.Children.Add(bview,0,0);
            var tgr = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            tgr.TappedCallback = async (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine(runcollect.OnboardTelemetryUrl);
                await Navigation.PushAsync(new NonLiveEventTelemetry(runcollect));
            };
            /*Row 0*/
            StackLayout sl1 = new StackLayout {
                Orientation = StackOrientation.Vertical
            };
            Label label1 = new Label();
            //label1.Text = String.Format("{0 : MMMM d, yyyy" ,runcollect.Timestamp);
            label1.Text = String.Format("{0:MMMM dd, yyyy HH:mm:ss}", runcollect.Timestamp);
            label1.TextColor = Color.White;
            Label label2 = new Label();
            label2.Text = runcollect.Pilot;
            label2.TextColor = Color.FromHex("#656472");
            sl1.Children.Add(label1);
            sl1.Children.Add(label2);
            sl1.Margin = new Thickness(25, 10, 5, 15);
            childGrid.BackgroundColor = Color.FromHex("#23232b");
            childGrid.Children.Add(sl1, 0, 0);
            /*Row 1*/
            StackLayout sl2 = new StackLayout {
                Orientation = StackOrientation.Vertical
                };
            Label label4 = new Label();
            label4.Text = "MAX AIR SPEED";
            label4.TextColor = Color.FromHex("#d90000");
            StackLayout sl3 = new StackLayout {
                Orientation = StackOrientation.Horizontal
            };
            Label label5 = new Label();
            label5.Text = (runcollect.maxSpeed).ToString(); ;
            label5.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            label5.TextColor = Color.White;

            Label label3 = new Label();
            label3.Text = "KPH";
            label3.VerticalOptions = LayoutOptions.End;
            label3.HorizontalOptions = LayoutOptions.Center;
            label3.TextColor = Color.FromHex("#656472");

            sl3.Children.Add(label5);
            sl3.Children.Add(label3);
            sl2.Children.Add(label4);
            sl2.Children.Add(sl3);
            sl2.Margin = new Thickness(25, 10, 5, 15);
            childGrid.Children.Add(sl2, 0, 1);

            /*Row 2*/
            StackLayout sl4 = new StackLayout { Orientation = StackOrientation.Horizontal };

            Grid sl4g1 = new Grid();
            sl4g1.Margin = new Thickness(0, 5, 15, 0);
            RoundedBoxView rbview = new RoundedBoxView();
            rbview.HeightRequest = fontsizelarge * 3;
            rbview.WidthRequest = fontsizelarge * 3;
            rbview.HorizontalOptions = LayoutOptions.Center;
            rbview.VerticalOptions = LayoutOptions.Center;
            //rbview.Margin = new Thickness(5, 5, 5, 5);
            Label maxacclx = new Label();
            maxacclx.Text = (runcollect.maxAccelX).ToString() + " g";
            maxacclx.TextColor = Color.White;
            maxacclx.HorizontalOptions = LayoutOptions.Center;
            maxacclx.VerticalOptions = LayoutOptions.Center;
            sl4g1.Children.Add(rbview);
            StackLayout slmaxacclx = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions=LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            Label accllabel = new Label();
            accllabel.Text = "ACCL";
            accllabel.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
            accllabel.TextColor = Color.FromHex("#656472");
            slmaxacclx.Children.Add(maxacclx);
            slmaxacclx.Children.Add(accllabel);
            //sl4g1.Children.Add(maxacclx);
            sl4g1.Children.Add(slmaxacclx);


            Grid sl4g2 = new Grid();
            sl4g2.Margin = new Thickness(0, 5, 15, 0);
            RoundedBoxView rbview1 = new RoundedBoxView();
            rbview1.HeightRequest = fontsizelarge * 3;
            rbview1.WidthRequest = fontsizelarge * 3;
            rbview1.HorizontalOptions = LayoutOptions.Center;
            rbview1.VerticalOptions = LayoutOptions.Center;
            //rbview1 = new Thickness(5, 5, 5, 5);
            Label sideaccly = new Label();
            sideaccly.Text = (runcollect.maxAccelY).ToString() + " g";
            sideaccly.TextColor = Color.White;
            sideaccly.HorizontalOptions = LayoutOptions.Center;
            sideaccly.VerticalOptions = LayoutOptions.Center;
            sl4g2.Children.Add(rbview1);
            StackLayout sideacclysl = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
            Label accllabely = new Label();
            accllabely.Text = "SIDE";
            accllabely.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
            accllabely.TextColor = Color.FromHex("#656472");
            sideacclysl.Children.Add(sideaccly);
            sideacclysl.Children.Add(accllabely);
            //sl4g1.Children.Add(maxacclx);
            sl4g2.Children.Add(sideacclysl);


            sl4.Margin = new Thickness(15, 5, 5, 15);
            sl4.Children.Add(sl4g1);
            sl4.Children.Add(sl4g2);
            childGrid.Children.Add(sl4, 0, 2);

            childGrid.Margin = new Thickness(0, 20, 0, 20);
            childGrid.GestureRecognizers.Add(tgr);
            ListRuns.Children.Add(childGrid);

            //childGrid.RowDefinitions.Add(new RowDefinition());
        }

        private async void ListViewItemSelection_Changed(object sender, SelectedItemChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("item selected");
            PreviousRunCollection runcollect = (PreviousRunCollection)e.SelectedItem;
            await Navigation.PushAsync(new NonLiveEventTelemetry(runcollect));
        }

        protected override void OnDisappearing()
        {
            ListRuns.Children.Clear();
            base.OnDisappearing();
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
