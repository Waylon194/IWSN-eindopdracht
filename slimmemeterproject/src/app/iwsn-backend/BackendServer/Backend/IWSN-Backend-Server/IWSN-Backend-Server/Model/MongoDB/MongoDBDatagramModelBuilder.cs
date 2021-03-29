using IWSN_Backend_Server.Model.Sensor;
using IWSN_Backend_Server.Models.Database;
using System;

namespace IWSN_Backend_Server.Model.Builders
{
    public class MongoDBDatagramModelBuilder
    {
        private MongoDBDatagramModel _Model;
        
        public MongoDBDatagramModelBuilder()
        {
            this._Model = new MongoDBDatagramModel();
            this._Model.DateOfMeasurement = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); // set the time on creating a new Model
        }

        public MongoDBDatagramModelBuilder SetMeasurementValue(ProcessedDatagram datagram)
        {
            this._Model.Datagram = datagram;
            return this;
        }

        public MongoDBDatagramModel CreateObject()
        {
            return this._Model;
        }
    }
}
