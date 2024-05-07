namespace Stackoverflow.Domain.Entities
{
    public class Post : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public PostType PostType { get; set; } 
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<Comment> Comments { get; set; }
    }

    public enum PostType
    {
        Question,
        Reply
    }
        
}
