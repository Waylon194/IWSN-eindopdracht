using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWSN_Backend_Server.Settings
{
    public class MQTTSettings
    {
        public const string BrokerHost = "plex.shitposts.nl";
        public const int BrokerPort = 1883;
        public const string UserName = "waylon";
        public const string Password = "waylon194";
        public const string Topic = "smartmeter/raw";
    }
}
