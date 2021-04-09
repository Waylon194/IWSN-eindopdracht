
namespace IWSN_Backend_Server.Model.Datagram
{
    public class ProcessedTelegram
    {
        public float PowerproductionTariff1 { get; set; }
        public string GasTimestamp { get; set; }
        public float GasUsage { get; set; }
        public float InstantaneousCurrent { get; set; }
        public float InstantaneousElectricityDelivery { get; set; }
        public float InstantaneousElectricityUsage { get; set; }
        public string CurrentTariffType { get; set; }
        public float PowerproductionTariff2 { get; set; }
        public float PowerConsumptionTariff2 { get; set; }
        public float PowerConsumptionTariff1 { get; set; }
    }
}
