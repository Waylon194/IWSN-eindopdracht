using IWSN_Backend_Server.Model.Sensor;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IWSN_Backend_Server.Models.Database
{
    public class MongoDBDatagramModel
    {
        /// <summary>
        /// <- Remark -> - When using the System.Text.Json make sure the attributes are reachable with { get; set; }!
        /// </summary>
        public MongoDBDatagramModel() { }

        // This is used for marking the MongoDB assigned unique id
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("DatagramDate")]
        public string DateOfMeasurement { get; set; }

        [BsonElement("DatagramValues")]
        public ProcessedDatagram Datagram { get; set; }
    }
}
