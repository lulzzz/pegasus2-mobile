using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PegasusData
{
    [JsonObject]
    public class UserMessage
    {
        public UserMessage()
        {

        }

        public static string UserMessageSerializer(UserMessage message)
        {
            return (JsonConvert.SerializeObject(message));
        } 

        public static UserMessage Load(byte[] message)
        {
            return JsonConvert.DeserializeObject<UserMessage>(Encoding.UTF8.GetString(message, 0, message.Length - 1));
        }

        public static byte[] ToCraftMessage(UserMessage umessage)
        {
            int v = (byte)Encoding.UTF8.GetBytes(prefix + umessage.Message).Sum(x => (int)x);
            string suffix = v.ToString("X2");
            string message = String.Format("{0}{1},*{2}", prefix, umessage.Message, suffix);
            return Encoding.UTF8.GetBytes(message);
        }

        public static UserMessage Load(string jsonString)
        {
            return JsonConvert.DeserializeObject<UserMessage>(jsonString);
        }

        private const string prefix = "{U:";

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
