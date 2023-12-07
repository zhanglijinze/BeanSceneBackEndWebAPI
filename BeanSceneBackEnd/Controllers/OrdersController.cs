using BeanSceneBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BeanSceneBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        // CRUD - create, read, update and delete
        MongoClient client;
        string databaseName;
        //constructor 
        public OrdersController(IOptions<BeanSceneDataBaseSettings> databaseSettings)
        {
            client = new MongoClient(databaseSettings.Value.ConnectionString);
            databaseName = databaseSettings.Value.DatabaseName;
        }
        //Get method is used to get all the orders from the mongodb database "Orders" collection
        [HttpGet]
        public IActionResult Get()
        {
            var collection = client.GetDatabase(databaseName).GetCollection<Order>("Order").AsQueryable();

            return collection == null ? NotFound() : Ok(collection);
        }
        //This method is used to add an order to the order list. 
        [HttpPost]
        public IActionResult Post(Order p)
        {
            var collection = client.GetDatabase(databaseName).GetCollection<Order>("Order").AsQueryable().OrderByDescending(c => c.orderNo).First();
            if (collection != null)
            {
                p.orderNo = Convert.ToInt32(collection.orderNo) + 1;
            }
            client.GetDatabase(databaseName).GetCollection<Order>("Order").InsertOne(p);
            return CreatedAtAction(nameof(Get), new { id = p._id }, p);
        }
        //this method is used to update the status of an order based on id       
        [HttpPut("{id}/status")]
        public IActionResult Put(string id, [FromBody] string status)
        {
            var filter = Builders<Order>.Filter.Eq("_id", id);
            if (filter is null)
            {
                return NotFound();
            }
            var update = Builders<Order>.Update.Set("status", status);


            client.GetDatabase(databaseName).GetCollection<Order>("Order").UpdateOne(filter, update);
            return Ok("Order updated");
        }




    }
}
