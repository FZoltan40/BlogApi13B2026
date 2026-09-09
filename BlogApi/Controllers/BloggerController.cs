using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=blog13b;uid=root;password=";
        
        [HttpGet]
        public List<Blogger> GetAllBlogger()
        {
            List<Blogger> bloggers = new();

            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = "SELECT * FROM blogger;";

            var cmd = new MySqlCommand(sql, connector);

            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var blogger = new Blogger
                {
                    Id = dataReader.GetInt32(0),
                    Name = dataReader.GetString(1),
                    Email = dataReader.GetString(2),
                    Age = dataReader.GetInt32(3),
                    Password = dataReader.GetString(4),
                    RegistrationTime = dataReader.GetDateTime(5)
                };

                bloggers.Add(blogger);
            }
           
            connector.Close();
            return bloggers;
        }

        [HttpPost]
        public object AddNewBlogger(Blogger blogger)
        {
            return null;
        }

        [HttpPut]
        public object UpdateBlogger(int id, Blogger blogger)
        {
            return null;
        }

        [HttpDelete]
        public object DeleteBlogger(int id) 
        {
            return null;
        }
    }
}
