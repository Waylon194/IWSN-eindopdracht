using IWSN_Backend_Server.Model.MongoDB;
using IWSN_Backend_Server.Models.Settings.Database;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace IWSN_Backend_Server.Services
{
    public class MongoDBTemperatureService
    {
        private readonly IMongoCollection<MongoDBTempModel> _TempSensorMeasurementDBCollection;

        private static MongoDBTemperatureService _Instance = null; // singleton instance object

        public static MongoDBTemperatureService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MongoDBTemperatureService(new IWSNDatabaseSettings());
                }
                return _Instance;
            }
        }

        public MongoDBTemperatureService(IWSNDatabaseSettings settings)
        {
            // Setting up the connection to the database
            MongoClient mongoDbClient = new MongoClient(settings.DBConnectionString); // connect to the MongoDB via the DB connection string 
            IMongoDatabase databaseData = mongoDbClient.GetDatabase(settings.DatabaseName); // get the IMongoDatabase object via the MongoDB client via a collection

            // Assign the db values to the readonly value
            this._TempSensorMeasurementDBCollection = databaseData.GetCollection<MongoDBTempModel>(settings.DBCollectionName_Tempature); // get the IMongoCollection object from the IMongoDatabase object
        }

        public Task<MongoDBTempModel> GetLatest()
        {
            return Task.FromResult<MongoDBTempModel>(this._TempSensorMeasurementDBCollection.AsQueryable().OrderByDescending(c => c.Id).First());
        }

        public void InsertTemperatureMeasurement(string data)
        {
            this._TempSensorMeasurementDBCollection.InsertOneAsync(new MongoDBTempModelBuilder()
                .SetMeasurementValue(data)
                .CreateObject());
        }

    }
}
