using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace PegasusNAEMobile.Collections
{
    public class ConfigCollection
    {
        public static RootObjectConfig DataDeserializer(string response)
        {
            RootObjectConfig rconfig = JsonConvert.DeserializeObject<RootObjectConfig>(response);
            return rconfig;
        }
    }

    [JsonObject]
    public class AggregateValues
    {
        public AggregateValues()
        {
        }

        [JsonProperty("maxSpeed")]
        public double MaxSpeed { get; set; }

        [JsonProperty("maxAccelX")]
        public double MaxAccelX { get; set; }

        [JsonProperty("maxAccelY")]
        public double MaxAccelY { get; set; }

        [JsonProperty("maxAccelZ")]
        public double MaxAccelZ { get; set; }


    }

    [JsonObject]
    public class CollectionConfig
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("runId")]
        public string RunId { get; set; }

        [JsonProperty("pilot")]
        public string Pilot { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("drone1VideoUrl")]
        public string Drone1VideoUrl { get; set; }

        [JsonProperty("drone2VideoUrl")]
        public string Drone2VideoUrl { get; set; }

        [JsonProperty("onboardVideoUrl")]
        public string OnboardVideoUrl { get; set; }

        [JsonProperty("onboardTelemetryUrl")]
        public string OnboardTelemetryUrl { get; set; }

        [JsonProperty("aggregateTelemetry")]
        public AggregateValues Aggregates { get; set; }
    }

    [JsonObject]
    public class RootObjectConfig
    {
        [JsonProperty("collection")]
        public List<CollectionConfig> collection { get; set; }
    }
}
