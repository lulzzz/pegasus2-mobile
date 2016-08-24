using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PegasusData
{
    public class Constants
    {
        public Constants()
        {

        }

        public static string TokenSecret
        {
            get { return "851o2LqnMUod9lp7DvVxSrH+KQAkydBF9MDREicDus4="; }
        }
        private static string _savedSecurityToken;
        public static string SavedSecurityToken
        {
            get { return _savedSecurityToken; }
            set { _savedSecurityToken = value; }
        }
        public static string TokenWebApiUri
        {
            get { return "https://authz.pegasusmission.io/api/phone"; }
        }

        public static string SubProtocol
        {
            get { return "coap.v1"; }
        }

        public static string LiveTelemetryHostUri
        {
            get { return ("ws://broker.pegasusmission.io/api/connect"); }
        }

        public static string TelemterySubscribeUri
        {
            get { return "coaps://pegasusmission.io/subscribe?topic=http://pegasusnae.org/telemetry"; }
        }

        public static string UserMessageUri
        {
            get { return "coaps://pegasusmission.io/publish?topic=http://pegasus2.org/usermessage"; }
        }
    }
}
