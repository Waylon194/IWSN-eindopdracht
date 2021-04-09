using IWSN_Backend_Server.Services;
using IWSN_Backend_Server.Settings;
using IWSN_Backend_Server.Settings.Mqtt;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IWSN_Backend_Server
{
    public class Program
    {
        static IManagedMqttClient mqtt_client_p1;
        static IManagedMqttClient mqtt_client_lm35;

        static MongoDBSmartMeterService sMService;
        static MongoDBTemperatureService tmpService;

        public static async Task Main(string[] args)
        {
            sMService = MongoDBSmartMeterService.Instance; // Create Mongodb smartmeter measurement service
            tmpService = MongoDBTemperatureService.Instance; // Create Mongodb temp measurement service

            mqtt_client_p1 = new MqttFactory().CreateManagedMqttClient();
            mqtt_client_lm35 = new MqttFactory().CreateManagedMqttClient();

            await MqttConnectAsync(MqttClientType.MQTT_P1);
            await MqttConnectAsync(MqttClientType.MQTT_LM35);
            
            // Run REST server API configurations
            CreateHostBuilder(args).Build().Run();
        }

        // Credit to => https://dzone.com/articles/mqtt-publishing-and-subscribing-messages-to-mqtt-b
        public static async Task MqttConnectAsync(MqttClientType type)
        {
            string clientId = Guid.NewGuid().ToString(); // create a random MQTT client ID
            string mqttURI = MQTTSettings.BrokerHost; // MQTT hostname
            int mqttPort = MQTTSettings.BrokerPort; // MQTT host port
            string mqttUser = MQTTSettings.UserName; // MQTT client username
            string mqttPassword = MQTTSettings.Password; // MQTT client password
            bool mqttSecure = false; // MQTT SSL connection toggle

            var messageBuilder = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithCredentials(mqttUser, mqttPassword)
                .WithTcpServer(mqttURI, mqttPort)
                .WithCleanSession();

            var options = mqttSecure
              ? messageBuilder
                .WithTls()
                .Build()
              : messageBuilder
                .Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder()
              .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
              .WithClientOptions(options)
              .Build();

            // create a new managed mqtt client for the mqtt messaging

            switch (type)
            {
                case MqttClientType.MQTT_P1:
                    mqtt_client_p1.UseApplicationMessageReceivedHandler(e =>
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(MQTTSettings.Topic_P1) == false)
                            {
                                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                                Console.WriteLine($"Topic: {MQTTSettings.Topic_P1}. Message Received: {payload}");

                                sMService.InsertDatagramMeasurement(payload);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message, ex);
                        }
                    });
                    await mqtt_client_p1.StartAsync(managedOptions);
                    await SubscribeAsync(mqtt_client_p1 ,MQTTSettings.Topic_P1); // Subscribe on topic
                    break;

                case MqttClientType.MQTT_LM35:
                    mqtt_client_lm35.UseApplicationMessageReceivedHandler(e =>
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(MQTTSettings.Topic_LM35) == false)
                            {
                                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                                if (payload.Length == 5)
                                {
                                    Console.WriteLine($"Topic: {MQTTSettings.Topic_LM35}. Message Received: {payload}");
                                    tmpService.InsertTemperatureMeasurement(payload);
                                }
                                else
                                {
                                    Console.WriteLine("ERROR : Sensor Desync issue occured filtering out measurement!");
                                }                              
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message, ex);
                        }
                    });
                    await mqtt_client_lm35.StartAsync(managedOptions);
                    await SubscribeAsync(mqtt_client_lm35, MQTTSettings.Topic_LM35); // Subscribe on topic
                    break;

                default:
                    Console.WriteLine("Error!");
                    throw new NotImplementedException();
            }           
        }

        // Quality of Service = 1 => "At least once"
        public static async Task SubscribeAsync(IManagedMqttClient client, string topic, int qos = 1)
        {
            await client.SubscribeAsync(new MqttTopicFilterBuilder()
            .WithTopic(topic)
            .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
            .Build());
        }

        // Start services & controllers (IWSNController) and other ASP.NET configurations
        // (defined in the Properties.lauchSettings.json)
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
