using MongoDB.Bson.Serialization.Attributes;
namespace BeanSceneBackEnd.Models

{
    public class OrderItem
    {
        [BsonId]//we are making this property _id as the primary
        [BsonElement("_id")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        // it is allowing to pass the parameter as type string instead of objectid, Mongodb will handle the conversation from string to objectid
        public string? productId { get; set; }
        public int quantity { get; set; }
        public string name { get; set; }

        public string? note { get; set; }    
    }
}
