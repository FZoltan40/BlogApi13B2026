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
        public object UpdatePost([FromQuery] int id, [FromBody] UpdatePostDto updatePostDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"UPDATE `blogpost` SET `title`=@title,`content`=@content,`updateTim`=@updateTime
                WHERE `id`= @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@title", updatePostDto.Title);
            cmd.Parameters.AddWithValue("@content", updatePostDto.Content);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            var updatedPost = new UpdatePostDto
            {
               Title = updatePostDto.Title,
               Content = updatePostDto.Content
            };

            connector.Close();

            return new { message = "Sikeres frissítés.", result = updatePostDto };
        }

        [HttpDelete]
        public object DeletePost(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = $"DELETE FROM blogpost WHERE id = @id";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue(@"id", id);

            cmd.ExecuteNonQuery();

            connector.Close();

            return new { message = "Sikeres tölrés" };
        }
    }
}
