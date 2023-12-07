using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace BeanSceneBackEnd.Models
{
    public class MenuItem
    {
        [BsonId]//we are making this property _id as the primary
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        // it is allowing to pass the parameter as type string instead of objectid, Mongodb will handle the conversation from string to objectid
        public string? _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string photo { get; set; }
        public bool availability { get; set; }
        public bool special { get; set; }
        public DietaryFlags dietaryFlags { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string categoryId { get; set; }
        public string categoryName { get; set; }

        public class DietaryFlags
        {
            [BsonElement("Gluten-free")]
            public bool GlutenFree { get; set; }

            [BsonElement("Dairy-free")]
            public bool DairyFree { get; set; }

            public bool Vegetarian { get; set; }
            public bool Vegan { get; set; }
            public bool Allergens { get; set; }

        }

    }
}
