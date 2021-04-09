
namespace IWSN_Backend_Server.Model.Sensor
{
    /// <summary>
    /// The DatagramShell class is neccesary for the json parsing (it comes inside a seperate object, the "DatagramShell")
    /// It functions as wrapper for the content of the RawDatagram class
    /// </summary>
    public class DatagramShell
    {        
        public RawDatagram datagram { get; set; }
    }
}
