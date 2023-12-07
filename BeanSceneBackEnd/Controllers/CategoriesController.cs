using BeanSceneBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BeanSceneBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        // CRUD - create, read, update and delete
        MongoClient client;
        string databaseName;
        //constructor 
        public CategoriesController(IOptions<BeanSceneDataBaseSettings> databaseSettings)
        {
            client = new MongoClient(databaseSettings.Value.ConnectionString);
            databaseName = databaseSettings.Value.DatabaseName;
        }
        //Get method is used to get all the categories from the mongodb database "Categories" collection
        [HttpGet]
        public IActionResult Get()
        {
            var collection = client.GetDatabase(databaseName).GetCollection<Category>("MenuCategory").AsQueryable();

            return collection == null ? NotFound() : Ok(collection);
        }
        [HttpPost]
        public IActionResult Post(Category C)
        {
            client.GetDatabase(databaseName).GetCollection<Category>("MenuCategory").InsertOne(C);
            return CreatedAtAction(nameof(Get), new { id = C._id }, C);
        }

        [HttpPut]
        public IActionResult Put(Category C)
        {
            var filter = Builders<Category>.Filter.Eq("_id", C._id);
            if (filter is null)
            {
                return NotFound();
            }
            var update = Builders<Category>.Update.Set("name", C.name);
            client.GetDatabase(databaseName).GetCollection<Category>("MenuCategory").UpdateOne(filter, update);
            var filterItem = Builders<MenuItem>.Filter.Eq("categoryId", C._id);
            var updateItem = Builders<MenuItem>.Update.Set("categoryName", C.name);
            client.GetDatabase(databaseName).GetCollection<MenuItem>("MenuItemsWithCategory").UpdateOne(filterItem, updateItem);
            return Ok("Category and MenuItem Updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var filterItem = Builders<MenuItem>.Filter.Eq("categoryId", id);
            var ItemCollection = client.GetDatabase(databaseName).GetCollection<MenuItem>("MenuItemsWithCategory");
            var filterItemCheck = ItemCollection.CountDocuments(filterItem);
            if (filterItemCheck > 0)
            {
                return Conflict("Cannot delete Category, it's used by at least one menu item.");
            }
            var collection = client.GetDatabase(databaseName).GetCollection<Category>("MenuCategory");
            var filter = Builders<Category>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
            return filter == null ? NotFound() : Ok(filter);
        }

    }
}
