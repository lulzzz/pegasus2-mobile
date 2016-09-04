using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PegasusNAEMobile.Collections;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace PegasusNAEMobile
{
    public partial class PreviousRunsList : ContentPage
    {
        public ObservableCollection<PreviousRunCollection> runlist { get; set; }
        public PreviousRunsList()
        {
            runlist = new ObservableCollection<PreviousRunCollection>();
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            string configjson = await App.Instance.GetFileFromBlob("https://pegasustest.blob.core.windows.net/pegasustestblob/config.json");
            RootObjectConfig rconfig = ConfigCollection.DataDeserializer(configjson);
            runlist.Clear();
            foreach (var config in rconfig.collection)
            {
                PreviousRunCollection runcollect = new PreviousRunCollection();
                runcollect.AggregateTelemtryUrl = config.AggregateTelemtryUrl;
                runcollect.Drone1VideoUrl = config.Drone1VideoUrl;
                runcollect.Drone2VideoUrl = config.Drone2VideoUrl;
                runcollect.Location = config.Location;
                runcollect.OnboardTelemetryUrl = config.OnboardTelemetryUrl;
                runcollect.OnboardVideoUrl = config.OnboardVideoUrl;
                runcollect.Pilot = config.Pilot;
                runcollect.RunId = config.RunId;
                runcollect.Timestamp = config.Timestamp;
                runlist.Add(runcollect);
            }
            PreviousRunListView.ItemsSource = runlist;
            base.OnAppearing();
        }

        private async void ListViewItemSelection_Changed(object sender, SelectedItemChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("item selected");
            PreviousRunCollection runcollect = (PreviousRunCollection)e.SelectedItem;
            await Navigation.PushAsync(new NonLiveEventTelemetry(runcollect));
        }
    }
}
