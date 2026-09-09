namespace BlogApi.Models.DTOs
{
    public class AddBloggerDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
        public string? Password { get; set; }
    }
}
