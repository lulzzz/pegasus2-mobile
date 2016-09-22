using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
namespace PegasusNAEMobile.PushNotifications
{
    public class PushNotificationManager
    {
        MobileServiceClient client;
        static PushNotificationManager defaultInstance = new PushNotificationManager();
        public static PushNotificationManager DefaultManager
        {
            get { return defaultInstance; }
            private set { defaultInstance = value; }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public PushNotificationManager()
        {
            this.client = new MobileServiceClient("http://pegasusnotification.azurewebsites.net");
            //this.userInfoTable = client.GetTable<UserInfo>();
        }
    }
}
