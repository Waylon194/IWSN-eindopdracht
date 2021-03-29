using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWSN_Backend_Server.Models.Settings.Database
{
    public class IWSNDatabaseSettings
    {   
        // Sensor database - IWSN - Intelligent Wireless Sensor Networks
        public string DBCollectionName = "SmartmeterMeasurements";
        public string DBConnectionString = "mongodb://localhost:27017";
        public string DatabaseName = "IWSN"; // Intelligent Wireless Sensor Networks
    }
}
