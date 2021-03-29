
namespace IWSN_Backend_Server.Model.Sensor
{
    /// <summary>
    /// Class used by the Raw/ProcessedDatagram classes. 
    /// Functions as a data-container for the car-charger and solar-panels
    /// </summary>
    public class SObject
    {
        public string unit { get; set; }
        public string label { get; set; }
        public int value { get; set; }
    }
}
