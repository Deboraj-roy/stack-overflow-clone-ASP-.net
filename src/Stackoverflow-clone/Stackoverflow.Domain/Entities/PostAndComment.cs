using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stackoverflow.Domain.Entities
{
    public class PostAndComment
    {
        public Guid PostId { get; set; }

        public Guid CommentId { get; set; }

    }
}
