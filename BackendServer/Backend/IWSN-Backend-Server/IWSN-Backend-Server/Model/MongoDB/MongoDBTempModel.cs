using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IWSN_Backend_Server.Model.MongoDB
{
    public class MongoDBTempModel
    {
        // This is used for marking the MongoDB assigned unique id
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("TempDate")]
        public string DateOfMeasurement { get; set; }

        [BsonElement("Tempature")]
        public string Tempature { get; set; }
    }
}
