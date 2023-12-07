using BeanSceneBackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BeanSceneBackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize("BasicAuthentication")]
    public class StaffController : ControllerBase
    {
        // CRUD - create, read, update and delete
        MongoClient client;
        string databaseName;
        //constructor 
        public StaffController(IOptions<BeanSceneDataBaseSettings> databaseSettings)
        {
            client = new MongoClient(databaseSettings.Value.ConnectionString);
            databaseName = databaseSettings.Value.DatabaseName;
        }
        //not hashed
        /*[HttpGet("{username}/{password}")]
        public IActionResult Get(string username, string password)
        {
            var collection = client.GetDatabase(databaseName).GetCollection<Staff>("Staff");
            var filteredResult = collection.Find(x => x.username == username && x.password == password).FirstOrDefault();
            return filteredResult == null ? NotFound() : Ok(filteredResult);
        }*/

        //hashed
        [HttpGet("{username}/{password}")]
        public IActionResult Get(string username, string password)
        {
            var collection = client.GetDatabase(databaseName).GetCollection<Staff>("Staff");
            var filteredResult = collection.Find(x => x.username == username).FirstOrDefault();
            if (filteredResult != null)
            {
                try
                {

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    bool isVerified = BCrypt.Net.BCrypt.Verify(password, filteredResult.password);
                    if (!isVerified)
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    return NotFound(ex);
                }
            }
            return filteredResult == null ? NotFound() : Ok(filteredResult);
        }



        //Get method is used to get all staff details from the mongodb database "staff" collection
        [HttpGet]
        public IActionResult Get()
        {
            var collection = client.GetDatabase(databaseName).GetCollection<Staff>("Staff").AsQueryable();

            return collection == null ? NotFound() : Ok(collection);
        }

        //Post methods - this is used to add a product into the products collection of mongodb database. 
        [HttpPost]
        public IActionResult Post(Staff s)
        {
            //hash the password and then save the hashed password in the staff table
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(s.password);
            s.password = hashedPassword;

            client.GetDatabase(databaseName).GetCollection<Staff>("Staff").InsertOne(s);
            return CreatedAtAction(nameof(Get), new { id = s._id }, s);
        }
        //Put method - this is used to update that exist inside the products collection of mongodb database
        [HttpPut]
        public IActionResult Put(Staff s)
        {
            var filter = Builders<Staff>.Filter.Eq("_id", s._id);
            if (filter is null)
            {
                return NotFound();
            }
            var update = Builders<Staff>.Update.Set("email", s.email).Set("firstName", s.firstName).Set("lastName", s.lastName).Set("password", s.password).Set("role", s.role).Set("username", s.username).Set("phone",s.phone);
            client.GetDatabase(databaseName).GetCollection<Staff>("Staff").UpdateOne(filter, update);
            return Ok("Staff Updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var collection = client.GetDatabase(databaseName).GetCollection<Staff>("Staff");
            var filter = Builders<Staff>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
            return filter == null ? NotFound() : Ok(filter);
        }


    }
}
