using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using System.Data;

namespace BlogApi.Controllers
{
    [Route("blogger")]
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
        public Blogger AddNewBlogger(AddBloggerDto blogger)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var blg = new Blogger
            {
                Name = blogger.Name,
                Email = blogger.Email,
                Age = blogger.Age,
                Password = blogger.Password,
                RegistrationTime = DateTime.Now
            };

            var sql = $"INSERT INTO `blogger`(`name`, `email`, `age`, `password`, `RegistrationTime`) VALUES (@name,@email,@age,@password,@registrationtime)";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@name", blg.Name);
            cmd.Parameters.AddWithValue("@email", blg.Email);
            cmd.Parameters.AddWithValue("@age", blg.Age);
            cmd.Parameters.AddWithValue("@password", blg.Password);
            cmd.Parameters.AddWithValue("@registrationtime", blg.RegistrationTime);

            cmd.ExecuteNonQuery();

            connector.Close();

            return blg;
        }

        [HttpPut]
        public object UpdateBlogger([FromQuery] int id, [FromBody] UpdateBloggerDto updateBloggerDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"UPDATE `blogger` SET `name`=@name,`email`=@email,`age`=@age,`password`=@password 
                WHERE `id`= @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@name", updateBloggerDto.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDto.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDto.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDto.Password);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            var updatedBlogger = new UpdateBloggerDto
            {
                Name = updateBloggerDto.Name,
                Email = updateBloggerDto.Email,
                Age = updateBloggerDto.Age,
                Password = updateBloggerDto.Password
            };

            connector.Close();

            return new { message = "Sikeres frissítés.", result = updatedBlogger };
        }

        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = $"DELETE FROM blogger WHERE id = @id";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue(@"id", id);

            cmd.ExecuteNonQuery();

            connector.Close();

            return new { message = "Sikeres tölrés" };
        }

        [HttpGet("byId")]
        public object GetBloggerById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT `name`, `email` FROM `blogger`
                        WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();
            datareader.Read();
            var blogger = new 
            {
                Name = datareader.GetString(0),
                Email = datareader.GetString(1)
            };

            connector.Close();

            return blogger;
        }

        [HttpGet("bloggerOwnPost")]
        public List<object> GetBloggerWithPost(int id)
        {
            List<object> ownPost = new List<object>();
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT blogger.name, blogpost.title, blogpost.content  
                        FROM `blogger` 
                        INNER JOIN blogpost ON blogger.id = blogpost.blogId
                        WHERE blogger.`id` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var bloggerOwnPosts = new
                {
                    Name = datareader.GetString(0),
                    Title = datareader.GetString(1),
                    Content = datareader.GetString(2)
                };

                ownPost.Add(bloggerOwnPosts);
            }

           

            connector.Close();

            return ownPost;
        }
    }
}

