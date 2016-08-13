using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PegasusNAEMobile
{
    public class CountdownViewModel
    {
        private string _timeToLiveEvent;   // asgangal : String object because it gives us the flexibility of binding a message,
                                           //instead of the time here, in case there is a delay.
        public string TimeToLiveEvent
        {
            get
            {
                if(String.IsNullOrEmpty(_timeToLiveEvent))
                {
                    _timeToLiveEvent = "-- : -- : --";
                }
                return _timeToLiveEvent;
            }
            private set { }
        }

        private string _heroMessage = "Pegasus Mission Goes Supersonic with NAE";
        public string HeroMessage
        {
            get { return _heroMessage; }
            private set { }
        }
    }
}
