using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWSN_Backend_Server.Model.Sensor
{
    /// <summary>
    /// Inner class of the DatagramShell
    /// Contains all the datagram data
    /// </summary>
    public class RawDatagram
    {
        public string p1 { get; set; } // datagram measurement values
        public string signature { get; set; } // signature of the datagram
        public SObject s0 { get; set; } // (Currently) car-charger object 
        public SObject s1 { get; set; } // (Currently) solar-panel object
    }
}
