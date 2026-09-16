namespace BlogApi.Models.DTOs
{
    public class AddPostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
    }
}
