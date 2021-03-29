using IWSN_Backend_Server.Model.Datagram;

namespace IWSN_Backend_Server.Model.Sensor
{
    /// <summary>
    /// Class which contains the processed (parsed/converted) (RawDatagram) model 
    /// </summary>
    public class ProcessedDatagram
    {
        public ProcessedTelegram Telegram { get; set; }
        public string Signature { get; set; } // signature of the datagram
        public SObject CarCharger { get; set; } // (Currently) car-charger object 
        public SObject SolarPanel { get; set; } // (Currently) solar-panel object
    }
}
