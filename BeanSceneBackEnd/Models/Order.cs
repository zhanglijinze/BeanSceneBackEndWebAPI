using MongoDB.Bson.Serialization.Attributes;
namespace BeanSceneBackEnd.Models
{
    public class Order
    {
        [BsonId]//we are making this property _id as the primary
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    // it is allowing to pass the parameter as type string instead of objectid, Mongodb will handle the conversation from string to objectid
    public string? _id { get; set; }
    public int orderNo { get; set; }
    public string status { get; set; }
    public string? note { get; set; }
    public string? name { get; set; }
    public DateTime dateTime { get; set; }
public string area { get; set; }
        public string table { get; set; }
    public List <OrderItem> productIds { get; set; }

    }
}
