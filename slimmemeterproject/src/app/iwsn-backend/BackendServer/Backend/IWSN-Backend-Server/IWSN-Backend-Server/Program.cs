using IWSN_Backend_Server.Services;
using IWSN_Backend_Server.Settings;
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
        static IManagedMqttClient client;
        static MongoDBService sMService;

        public static async Task Main(string[] args)
        {
            await MqttConnectAsync();
            await SubscribeAsync(MQTTSettings.Topic); // Subscribe on topic
            
            // Run REST server API configurations
            CreateHostBuilder(args).Build().Run();
        }

        // Credit to => https://dzone.com/articles/mqtt-publishing-and-subscribing-messages-to-mqtt-b
        public static async Task MqttConnectAsync()
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

            sMService = MongoDBService.Instance; // Create Mongodb sensor measurement service

            client = new MqttFactory().CreateManagedMqttClient();

            client.UseApplicationMessageReceivedHandler(e => 
            {
                try
                {
                    string topic = e.ApplicationMessage.Topic;

                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        Console.WriteLine($"Topic: {topic}. Message Received: {payload}");

                        sMService.insertDatagramMeasurement(payload);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            });
            await client.StartAsync(managedOptions);
        }

        // Quality of Service = 1 => "At least once"
        public static async Task SubscribeAsync(string topic, int qos = 1)
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
