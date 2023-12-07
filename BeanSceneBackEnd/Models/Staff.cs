using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BeanSceneBackEnd.Models
{
    public class Staff
    {
        [BsonId]//we are making this property _id as the primary
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        // it is allowing to pass the parameter as type string instead of objectid, Mongodb will handle the conversation from string to objectid
        public string? _id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string phone { get; set; }
    }
}
