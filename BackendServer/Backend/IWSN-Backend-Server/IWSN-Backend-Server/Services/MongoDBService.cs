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
    public class MongoDBService
    {
        private readonly IMongoCollection<MongoDBDatagramModel> _SensorMeasurementDBCollection;

        // parsers and processors
        private Parser _SmartMeterParser;
        private TelegramProcessor processor;

        private static MongoDBService _Instance = null; // singleton instance object

        public static MongoDBService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MongoDBService(new IWSNDatabaseSettings());
                }
                return _Instance;
            }
        }

        public MongoDBService(IWSNDatabaseSettings settings)
        {
            // Setting up the connection to the database
            MongoClient mongoDbClient = new MongoClient(settings.DBConnectionString); // connect to the MongoDB via the DB connection string 
            IMongoDatabase databaseData = mongoDbClient.GetDatabase(settings.DatabaseName); // get the IMongoDatabase object via the MongoDB client via a collection

            // Assign the db values to the readonly value
            this._SensorMeasurementDBCollection = databaseData.GetCollection<MongoDBDatagramModel>(settings.DBCollectionName); // get the IMongoCollection object from the IMongoDatabase object

            // Create the parser & processor
            this._SmartMeterParser = new Parser();
            this.processor = new TelegramProcessor();
        }

        public void insertDatagramMeasurement(string json)
        {
            var datagramShell = JsonSerializer.Deserialize<DatagramShell>(json);
            var result = this._SmartMeterParser.Parse(datagramShell.datagram.p1).Result.ToList().First();
            Console.WriteLine();

            ProcessedDatagram pDatagram = new ProcessedDatagram();
            pDatagram.Telegram = processor.Process(result);
            pDatagram.Signature = datagramShell.datagram.signature;
            pDatagram.CarCharger = datagramShell.datagram.s0;
            pDatagram.SolarPanel = datagramShell.datagram.s1;

            this._SensorMeasurementDBCollection.InsertOneAsync(new MongoDBDatagramModelBuilder()
                .SetMeasurementValue(pDatagram)
                .CreateObject());
        }

        // get all database entries
        public Task<IEnumerable<MongoDBDatagramModel>> GetAllAsync()
        {
            // returns the user if it was found
            return Task.FromResult<IEnumerable<MongoDBDatagramModel>>(this._SensorMeasurementDBCollection.Find(sensor => true).ToList());
        }

        public IEnumerable<MongoDBDatagramModel> GetAll()
        {
            return this._SensorMeasurementDBCollection.Find(sensor => true).ToList();
        }
    }
}
