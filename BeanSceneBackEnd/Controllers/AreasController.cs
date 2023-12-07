using BeanSceneBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BeanSceneBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        // CRUD - create, read, update and delete
        MongoClient client;
        string databaseName;
        //constructor 
        public AreasController(IOptions<BeanSceneDataBaseSettings> databaseSettings)
        {
            client = new MongoClient(databaseSettings.Value.ConnectionString);
            databaseName = databaseSettings.Value.DatabaseName;
        }
        //Get method is used to get all the areas and table from the mongodb database "AreaWithTable" collection

        [HttpGet]
        public IActionResult Get()
        {
            var collection = client.GetDatabase(databaseName).GetCollection<Area>("AreaWithTable").AsQueryable();

            return collection == null ? NotFound() : Ok(collection);
        }
    }
}
