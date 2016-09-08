using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegasusNAEMobile.Collections
{
    public class PreviousRunCollection
    {
        public DateTime Timestamp { get; set; }
        public string RunId { get; set; }
        public string Pilot { get; set; }
        public string Location { get; set; }
        public string Drone1VideoUrl { get; set; }
        public string Drone2VideoUrl { get; set; }
        public string OnboardVideoUrl { get; set; }
        public string OnboardTelemetryUrl { get; set; }
        public double maxAccelX { get; set; }
        public double maxAccelY { get; set; }
        public double maxAccelZ { get; set; }
        public double maxSpeed { get; set; }
    }

     
}
