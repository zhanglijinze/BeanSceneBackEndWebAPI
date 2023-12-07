using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BeanSceneBackEnd.Models;

namespace BeanSceneBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        // CRUD - create, read, update and delete
        MongoClient client;
        string databaseName;
        //constructor 
        public MenuItemsController(IOptions<BeanSceneDataBaseSettings> databaseSettings)
        {
            client = new MongoClient(databaseSettings.Value.ConnectionString);
            databaseName = databaseSettings.Value.DatabaseName;
        }
        //Get method is used to get all the products from the mongodb database "products" collection
        [HttpGet]
        public IActionResult Get()
        {
            var collection = client.GetDatabase(databaseName).GetCollection<MenuItem>("MenuItemsWithCategory").AsQueryable();

            return collection == null ? NotFound() : Ok(collection);
        }

        //Post methods - this is used to add a product into the products collection of mongodb database. 
        [HttpPost]
        public IActionResult Post(MenuItem I)
        {
            client.GetDatabase(databaseName).GetCollection<MenuItem>("MenuItemsWithCategory").InsertOne(I);
            return CreatedAtAction(nameof(Get), new { id = I._id }, I);
        }
        //Put method - this is used to update that exist inside the products collection of mongodb database
        [HttpPut]
        public IActionResult Put(MenuItem I)
        {
            var filter = Builders<MenuItem>.Filter.Eq("_id", I._id);
           /* if (filter == null)
            {
                return NotFound();
            }*/
            var update = Builders<MenuItem>.Update.Set("name", I.name).Set("price", I.price).Set("description", I.description).Set("availability", I.availability).Set("categoryId", I.categoryId).Set("categoryName",I.categoryName).Set("dietaryFlags",I.dietaryFlags).Set("photo",I.photo).Set("special",I.special);
           var result = client.GetDatabase(databaseName).GetCollection<MenuItem>("MenuItemsWithCategory").UpdateOne(filter, update);
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }
            return Ok("MenuItemUpdated");
        }
        //Delete method - this is used delete a product from the collection based on id
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var collection = client.GetDatabase(databaseName).GetCollection<MenuItem>("MenuItemsWithCategory");
            var filter = Builders<MenuItem>.Filter.Eq("_id", id);
           var result = collection.DeleteOne(filter);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }
            /*return (filter == null ? NotFound() : Ok(filter));*/
            return Ok("MenuItemDeleted");
        }

        [HttpGet("{nameMatch}")]
        public IActionResult Get(string nameMatch)
        {
            var collection = client.GetDatabase(databaseName).GetCollection<MenuItem>("MenuItemsWithCategory");
            var filteredList = collection.AsQueryable().Where(x => (x.name.ToLower()).Contains(nameMatch.ToLower()));
            return filteredList == null ? NotFound() : Ok(filteredList);
        }
    }
}
