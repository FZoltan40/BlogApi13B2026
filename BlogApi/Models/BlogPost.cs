namespace BlogApi.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int BlogId { get; set; }
    }
}
