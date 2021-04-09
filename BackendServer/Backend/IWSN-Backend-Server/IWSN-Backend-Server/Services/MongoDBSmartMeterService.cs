using DsmrParser.Dsmr;
using IWSN_Backend_Server.Model.Builders;
using IWSN_Backend_Server.Models.Database;
using IWSN_Backend_Server.Models.Settings.Database;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using IWSN_Backend_Server.Model.Sensor;
using IWSN_Backend_Server.Model.Datagram;

namespace IWSN_Backend_Server.Services
{
    /// <summary>
    /// This class handles the Mongo database and MQTT link
    /// -- Gets a MQTT measurement every 20s
    /// </summary>
    public class MongoDBSmartMeterService
    {
        private readonly IMongoCollection<MongoDBDatagramModel> _SmartMeterMeasurementDBCollection;

        // parsers and processors
        private Parser _SmartMeterParser;
        private TelegramProcessor processor;

        private static MongoDBSmartMeterService _Instance = null; // singleton instance object

        public static MongoDBSmartMeterService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MongoDBSmartMeterService(new IWSNDatabaseSettings());
                }
                return _Instance;
            }
        }

        public MongoDBSmartMeterService(IWSNDatabaseSettings settings)
        {
            // Setting up the connection to the database
            MongoClient mongoDbClient = new MongoClient(settings.DBConnectionString); // connect to the MongoDB via the DB connection string 
            IMongoDatabase databaseData = mongoDbClient.GetDatabase(settings.DatabaseName); // get the IMongoDatabase object via the MongoDB client via a collection

            // Assign the db values to the readonly value
            this._SmartMeterMeasurementDBCollection = databaseData.GetCollection<MongoDBDatagramModel>(settings.DBCollectionName_SmartMeter); // get the IMongoCollection object from the IMongoDatabase object

            // Create the parser & processor
            this._SmartMeterParser = new Parser();
            this.processor = new TelegramProcessor();
        }

        public void InsertDatagramMeasurement(string json)
        {
            var datagramShell = JsonSerializer.Deserialize<DatagramShell>(json);
            var result = this._SmartMeterParser.Parse(datagramShell.datagram.p1).Result.ToList().First();

            ProcessedDatagram pDatagram = new ProcessedDatagram();
            pDatagram.Telegram = processor.Process(result);
            pDatagram.Signature = datagramShell.datagram.signature;
            pDatagram.CarCharger = datagramShell.datagram.s0;
            pDatagram.SolarPanel = datagramShell.datagram.s1;

            this._SmartMeterMeasurementDBCollection.InsertOneAsync(new MongoDBDatagramModelBuilder()
                .SetMeasurementValue(pDatagram)
                .CreateObject());
        }

        // get all database entries
        public Task<IEnumerable<MongoDBDatagramModel>> GetAllAsync()
        {
            // returns the user if it was found
            return Task.FromResult<IEnumerable<MongoDBDatagramModel>>(this._SmartMeterMeasurementDBCollection.Find(sensor => true).ToList());
        }

        public IEnumerable<MongoDBDatagramModel> GetAll()
        {
            return this._SmartMeterMeasurementDBCollection.Find(sensor => true).ToList();
        }
    }
}
