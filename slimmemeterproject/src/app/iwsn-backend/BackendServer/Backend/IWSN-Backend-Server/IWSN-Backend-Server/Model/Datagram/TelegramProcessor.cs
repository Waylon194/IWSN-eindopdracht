using DsmrParser.Models;

namespace IWSN_Backend_Server.Model.Datagram
{
    public class TelegramProcessor
    {
        public ProcessedTelegram Process(Telegram telegram)
        {
            var pTelegram = new ProcessedTelegram();

            // production tarifs
            pTelegram.PowerproductionTariff1 = (float)telegram.PowerproductionTariff1;
            pTelegram.PowerproductionTariff2 = (float)telegram.PowerproductionTariff2;

            // consumptions tarifs
            pTelegram.PowerConsumptionTariff1 = (float) telegram.PowerConsumptionTariff1;
            pTelegram.PowerConsumptionTariff2 = (float) telegram.PowerConsumptionTariff2;

            // gas timestamp and usage // ToString("dd/MM/yyyy HH:mm:ss")
            pTelegram.GasTimestamp = telegram.GasTimestamp.ToString("dd/MM/yyyy HH:mm:ss");
            pTelegram.GasUsage = (float) telegram.GasUsage;

            // electricity usage
            pTelegram.InstantaneousCurrent = (float) telegram.InstantaneousCurrent;
            pTelegram.InstantaneousElectricityDelivery = (float) telegram.InstantaneousElectricityDelivery;
            pTelegram.InstantaneousElectricityUsage = (float) telegram.InstantaneousElectricityUsage;

            // assign the propper type
            switch (telegram.CurrentTariff)
            {
                case PowerTariff.Low:
                    pTelegram.CurrentTariffType = "Low";
                    break;
                case PowerTariff.Normal:
                    pTelegram.CurrentTariffType = "Normal";
                    break;
                default:
                    break;
            }

            return pTelegram;
        }
    }
}
