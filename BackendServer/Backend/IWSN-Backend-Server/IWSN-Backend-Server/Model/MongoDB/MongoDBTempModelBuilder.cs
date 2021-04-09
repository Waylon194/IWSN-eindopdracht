using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWSN_Backend_Server.Model.MongoDB
{
    public class MongoDBTempModelBuilder
    {
        private MongoDBTempModel _Model;

        public MongoDBTempModelBuilder()
        {
            this._Model = new MongoDBTempModel(); // cre
            this._Model.DateOfMeasurement = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); // set the time on creating a new Model
        }

        public MongoDBTempModelBuilder SetMeasurementValue(string temp)
        {
            this._Model.Tempature = temp;
            return this;
        }

        public MongoDBTempModel CreateObject()
        {
            return this._Model;
        }
    }
}
