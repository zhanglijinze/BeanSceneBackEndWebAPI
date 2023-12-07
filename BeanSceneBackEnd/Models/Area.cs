using MongoDB.Bson.Serialization.Attributes;
namespace BeanSceneBackEnd.Models
{
    public class Area
    {
        [BsonId]//we are making this property _id as the primary
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        // it is allowing to pass the parameter as type string instead of objectid, Mongodb will handle the conversation from string to objectid

        public string? _id { get; set; }
        public string areaName { get; set; }
        public List<string> tables { get; set; }

    }
}
