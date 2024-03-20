using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Domain.Entities
{
    public class Post : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public PostType PostType { get; set; }
        public Guid? ParentPostId { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum PostType
    {
        Question,
        Reply
    }
        
}
