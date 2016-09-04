using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegasusNAEMobile.ViewModels.Pages
{
    public class TelemetryViewModel : BaseViewModel
    {
        public LiveTelemetryViewModel LiveTelemetry
        {
            get;
            private set;
        }

        public TelemetryViewModel()
        {
            LiveTelemetry = App.Instance.CurrentVehicleTelemetry;
        }
    }
}
