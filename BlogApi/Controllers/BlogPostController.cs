using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Formats.Tar;

namespace BlogApi.Controllers
{
    [Route("post")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=blog13b;uid=root;password=";

        [HttpGet]
        public List<BlogPost> GetAllPost()
        {
            List<BlogPost> posts = new();

            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = "SELECT * FROM blogpost;";

            var cmd = new MySqlCommand(sql, connector);

            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var post = new BlogPost
                {
                    Id = dataReader.GetInt32(0),
                    Title = dataReader.GetString(1),
                    Content = dataReader.GetString(2),
                    PostTime = dataReader.GetDateTime(3),
                    UpdateTime = dataReader.GetDateTime(4)
                };

                posts.Add(post);
            }

            connector.Close();
            return posts;
        }

        [HttpPost]
        public BlogPost AddNewPost(AddPostDto addPostDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var post = new BlogPost
            {
              Title = addPostDto.Title,
              Content = addPostDto.Content,
              PostTime = DateTime.Now,
              UpdateTime = DateTime.Now,
              BlogId    = addPostDto.BlogId

            };

            var sql = $"INSERT INTO `blogpost`(`title`, `content`, `postTime`, `updateTim`, `blogId`) VALUES (@title,@content,@postTime,@updateTime,@blogId)";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@title", post.Title);
            cmd.Parameters.AddWithValue("@content", post.Content);
            cmd.Parameters.AddWithValue("@postTime", post.PostTime);
            cmd.Parameters.AddWithValue("@updateTime", post.UpdateTime);
            cmd.Parameters.AddWithValue("@blogId", post.BlogId);

            cmd.ExecuteNonQuery();

            connector.Close();

            return post;
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
    }
}
