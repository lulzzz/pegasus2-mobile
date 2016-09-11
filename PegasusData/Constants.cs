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

        private static int _screenHeight;
        public static int ScreenHeight
        {
            get { return _screenHeight; }
            set { _screenHeight = value; }
        }

        private static int _screenWidth;
        public static int ScreenWidth
        {
            get { return _screenWidth; }
            set { _screenWidth = value; }
        }
        public static string UserMessageTopicUri
        {
            get { return "coaps://pegasusmission.io/publish?topic=http://pegasus2.org/usermessage"; }
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

        public static string TelemteryPublishUri
        {
            get { return "coaps://pegasusmission.i/publis?topic=http://pegasusnae.org/telemetr"; }
        }

        public static string UserMessageUri
        {
            get { return "coaps://pegasusmission.io/publish?topic=http://pegasus2.org/usermessage"; }
        }

        public static string ConfigBlobFileUri
        {
            get { return "https://pegasus2.blob.core.windows.net/config/config.json"; }
        }
    }
}
