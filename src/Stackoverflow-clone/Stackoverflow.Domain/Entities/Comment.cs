using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Domain.Entities
{
    public class Comment : IEntity<Guid>
    {
        public Guid Id { get; set; } // unique identifier
        public Guid UserId { get; set; } // foreign key referencing the User who posted it
        public Guid PostId { get; set; } // foreign key referencing the Post it's commenting on
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
