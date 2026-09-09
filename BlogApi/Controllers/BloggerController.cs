using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        [HttpGet]
        public List<Blogger> GetAllBlogger()
        {
            return null;
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
